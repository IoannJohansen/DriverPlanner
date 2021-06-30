using Driver_Planner.Command;
using Driver_Planner.Model;
using Driver_Planner.Models;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class TimeTableViewModel : ViewModel
	{
		public TimeTableViewModel()
		{
			#region Data init
			DriverPlannerServiceClient dps = new DriverPlannerServiceClient();
			var listInst = dps.GetInstructors();
			if (listInst!=null && listInst.Length != 0)
			{
				ListInstructors = new ObservableCollection<Instructor>(listInst);
				var tmpTask = dps.GetClasses(_selectedDate, ListInstructors[SelectedInstructor].InstructorID);
				if (tmpTask != null)
				{
					TaskList = new ObservableCollection<TimeTable>(tmpTask);
				}
			}
			SelectedDate = DateTime.Today;
			#endregion

			#region Commands init
			PlanTaskCommand = new LambdaCommand(OnExecutePlanTaskCommand, CanExecutePlanTaskCommand);

			#endregion

			switch (CurrentUserSingleton.CurrentRole)
			{
				case ERole.USER:
					PickTaskButtonVisibility = Visibility.Visible;
					break;
				case ERole.INSTRUCTOR:
					PickTaskButtonVisibility = Visibility.Collapsed;
					break;
				case ERole.ADMIN:
					PickTaskButtonVisibility = Visibility.Collapsed;
					break;
				default:
					break;
			}

		}

		#region Props

		private Visibility _pickTaskButtonVisibility;

		public Visibility PickTaskButtonVisibility
		{
			get { return _pickTaskButtonVisibility; }
			set { Set(ref _pickTaskButtonVisibility, value); }
		}

		private ObservableCollection<Instructor> _listInstructor;

		public ObservableCollection<Instructor> ListInstructors
		{
			get { return _listInstructor; }
			set { Set(ref _listInstructor, value); }
		}

		private DateTime _selectedDate;

		public DateTime SelectedDate
		{
			get 
			{ 
				return _selectedDate; 
			}
			set 
			{ 
				Set(ref _selectedDate, value);
				if (ListInstructors != null && ListInstructors.Count != 0)
				{
					using (DriverPlannerServiceClient dps = new DriverPlannerServiceClient())
					{
						TaskList = new ObservableCollection<TimeTable>(dps.GetClasses(_selectedDate, ListInstructors[SelectedInstructor].InstructorID));
						SelectedIndexOfTask = -1;
					}
				}
			}
		}

		private TimeTable _selectedClass;

		public TimeTable SelectedClass
		{
			get 
			{ 
				return _selectedClass; 
			}
			set 
			{ 
				Set(ref _selectedClass, value);
			}
		}

		private int _selectedIndex;

		public int SelectedIndexOfTask
		{
			get { return _selectedIndex; }
			set 
			{
				if (value < 0)
				{
					Set(ref _selectedIndex, -1);
					SelectedClass = null;
				}
				else
				{
					Set(ref _selectedIndex, value);
					SelectedClass = TaskList[SelectedIndexOfTask];
				}
			}
		}

		private int _selectedInstructor = 0;

		public int SelectedInstructor
		{
			get { return _selectedInstructor; }
			set 
			{ 
				Set(ref _selectedInstructor, value);
				SelectedDate = SelectedDate;
			}
		}

		private ObservableCollection<TimeTable> _taskList;

		public ObservableCollection<TimeTable> TaskList
		{
			get { return _taskList; }
			set 
			{
				Set(ref _taskList, value); 
			}
		}


		#endregion

		#region Commands

		#region PlanTaskCommand

		public ICommand PlanTaskCommand { get; }
		private bool CanExecutePlanTaskCommand(object p)
		{
			return (SelectedClass != null && TaskList.Count != 0 && SelectedIndexOfTask != -1 && SelectedDate > DateTime.Today) ? true : false;
		}

		private void OnExecutePlanTaskCommand(object p)
		{
			try
			{
				using (var dps = new DriverPlannerServiceClient())
				{
					SelectedClass.UserID = ((User)CurrentUserSingleton.СurrentUser).UserID;
					if (dps.CheckForTaskLimits(((User)CurrentUserSingleton.СurrentUser).UserID, SelectedClass.DateOfClass))
					{
						if (dps.PickTask(SelectedClass, CurrentUserSingleton.СurrentUser as User))
						{
							SelectedDate = SelectedDate;
						}
					}
				}
			}
			catch (FaultException<InvalidOperationException> ex)
			{
				MessageBox.Show(ex.Message);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

		#endregion
	}
}
