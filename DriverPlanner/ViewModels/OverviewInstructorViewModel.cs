using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Driver_Planner.ViewModels
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
