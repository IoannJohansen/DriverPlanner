using System.Windows;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;

namespace DriverPlanner.ViewModels
{
	class OverviewInstructorViewModel : ViewModel
	{
		public OverviewInstructorViewModel(Instructor instance)
		{
			Instance = instance;
			ContentVisibility = Visibility.Visible;

		}

		#region Props

		private Visibility _contentVisibility;

		public Visibility ContentVisibility
		{
			get { return _contentVisibility; }
			set { Set(ref _contentVisibility, value); }
		}
		#endregion

		public OverviewInstructorViewModel()
		{
			ContentVisibility = Visibility.Hidden;
		}

		private Instructor _instance;

		public Instructor Instance
		{
			get { return _instance; }
			set { Set(ref _instance, value); }
		}



	}
}
