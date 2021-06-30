using Driver_Planner.ViewModels.Base;

namespace Driver_Planner.ViewModels
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
