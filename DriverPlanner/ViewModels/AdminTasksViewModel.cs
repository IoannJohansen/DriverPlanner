using System.Collections.ObjectModel;
using DriverPlanner.Data;
using DriverPlanner.Entities;
using DriverPlanner.Entities;
using DriverPlanner.Models.Classes;
using DriverPlanner.ViewModels.Base;

namespace DriverPlanner.ViewModels
{
	class AdminTasksViewModel : ViewModel
	{
		public AdminTasksViewModel()
		{
			using (var dps = new DriverPlannerService())
			{
				AllTasks = new ObservableCollection<TimeTable>(dps.GetMyTasks(null, (int)CurrentUserSingleton.CurrentRole));
				
			}

		}

		#region Props

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

		private byte[] _userImage;

		public byte[] UserImage
		{
			get { return _userImage; }
			set { Set(ref _userImage, value); }
		}

		private ObservableCollection<TimeTable> _allTasks;

		public ObservableCollection<TimeTable> AllTasks
		{
			get { return _allTasks; }
			set
			{
				Set(ref _allTasks, value);
				SelectedClass = (_allTasks != null && _allTasks.Count != 0) ? _allTasks[0] : null;
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
				}
				else
				{
					Set(ref _selectedIndex, value);
					SelectedClass = AllTasks[_selectedIndex];
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
					using (var dps = new DriverPlannerService())
					{
						CarImage = dps.GetImage(_selectedClass.Instructor.Car.ImageIndex);
						InstructorImage = dps.GetImage(_selectedClass.Instructor.ImageIndex);
						UserImage = dps.GetImage(_selectedClass.User.ImageIndex);
					}
				}
				else
				{
					CarImage = null;
					InstructorImage = null;
					UserImage = null;
				}
			}
		}

		#endregion

		

	}
}
