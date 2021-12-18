using DriverPlanner.ViewModels.Base;
using DriverPlanner.ViewModels;

namespace DriverPlanner.ViewModels
{
	class ProfileViewModel : ViewModel
	{
		public ProfileViewModel()
		{
			_currentViewModel = new ViewProfileViewModel(this);
		}

		private ViewModel _currentViewModel;

		public ViewModel CurrentProfileMode
		{
			get { return _currentViewModel; }
			set { Set(ref _currentViewModel, value); }
		}

	}
}
