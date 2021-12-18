using System.Collections.ObjectModel;
using DriverPlanner.Data;
using DriverPlanner.Entities;
using DriverPlanner.Models.Classes;
using DriverPlanner.ViewModels.Base;

namespace DriverPlanner.ViewModels
{
	class InstructorTaskViewModel : ViewModel
	{
		public InstructorTaskViewModel()
		{
			using (var dps =  new DriverPlannerService())
			{
				TaskList = new ObservableCollection<TimeTable>(dps.GetMyTasks(CurrentUserSingleton.СurrentUser, (int)CurrentUserSingleton.CurrentRole));
				if (TaskList != null && TaskList.Count != 0) SelectedIndex = 0;


			}
		}

		#region Props

		private ObservableCollection<TimeTable> _taskList;

		public ObservableCollection<TimeTable> TaskList
		{
			get { return _taskList; }
			set
			{ 
				Set(ref _taskList, value);
				if (TaskList != null && TaskList.Count != 0) SelectedIndex = 0;
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
					Set(ref _selectedIndex, -1);
					SelectedTask = null;
				}
				else
				{
					Set(ref _selectedIndex, value);
					SelectedTask = TaskList[_selectedIndex];
				}
			}
		}

		private TimeTable _selectedTask;

		public TimeTable SelectedTask
		{
			get { return _selectedTask; }
			set 
			{
				Set(ref _selectedTask, value);
			}
		}

		#endregion

	}
}
