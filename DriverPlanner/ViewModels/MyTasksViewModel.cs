using Driver_Planner.Command;
using Driver_Planner.Models;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class MyTasksViewModel : ViewModel
	{
		public MyTasksViewModel()
		{
			#region DataInit
			using (var dps = new DriverPlannerServiceClient())
			{
				MyTasks = new ObservableCollection<TimeTable>(dps.GetMyTasks((User)CurrentUserSingleton.СurrentUser, (int)CurrentUserSingleton.CurrentRole));
				SelectedIndex = -1;

			}
			#endregion

			#region Commands init

			CanvelTaskCommand = new LambdaCommand(OnExecuteCanvelTaskCommand, CanExecuteCanvelTaskCommand);

			#endregion

		}

		#region Props

		private Visibility _contentVisibility = Visibility.Hidden;

		public Visibility ContentVisibility
		{
			get { return _contentVisibility; }
			set { Set(ref _contentVisibility, value); }
		}

		private byte[] _carImage;

		public byte[] CarImage
		{
			get { return _carImage; }
			set { Set(ref _carImage, value); }
		}

		private byte[] _instructorImage;

		public byte[] InstructorImage
		{
			get { return _instructorImage; }
			set { Set(ref _instructorImage, value); }
		}

		private ObservableCollection<TimeTable> _myTasks;

		public ObservableCollection<TimeTable> MyTasks
		{
			get { return _myTasks; }
			set 
			{ 
				Set(ref _myTasks, value);
				SelectedClass = (_myTasks != null && _myTasks.Count != 0) ? _myTasks[0] : null;
			}
		}

		private int _selectedIndex;

		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set 
			{
				if (value < 0)
				{
					Set(ref _selectedIndex, value);
					SelectedClass = null;
					ContentVisibility = Visibility.Hidden;
				}
				else
				{
					Set(ref _selectedIndex, value);
					SelectedClass = MyTasks[_selectedIndex];
					ContentVisibility = Visibility.Visible;
				}
			}
		}

		private TimeTable _selectedClass;

		public TimeTable SelectedClass
		{
			get { return _selectedClass; }
			set
			{
				Set(ref _selectedClass, value);
				if (_selectedClass!=null)
				{
					using (var dps = new DriverPlannerServiceClient())
					{
						InstructorImage = dps.GetImage(_selectedClass.Instructor.ImageIndex);
						CarImage = dps.GetImage(_selectedClass.Instructor.Car.ImageIndex);

					}
				}
				else
				{
					CarImage = null;
					InstructorImage = null;
				}

			}
		}

		#endregion

		#region Commands

		#region CancelTaskCommand

		public ICommand CanvelTaskCommand { get; }
		private bool CanExecuteCanvelTaskCommand(object p) => (MyTasks != null && SelectedIndex != -1 && MyTasks.Count != 0) ? true : false;
		private void OnExecuteCanvelTaskCommand(object p)
		{
			DriverPlannerServiceClient dps = new DriverPlannerServiceClient();
			dps.CancelTask(SelectedClass.ClassID);
			MyTasks = new ObservableCollection<TimeTable>(dps.GetMyTasks((User)CurrentUserSingleton.СurrentUser, (int)CurrentUserSingleton.CurrentRole));

		}
		#endregion



		#endregion
	}
}
