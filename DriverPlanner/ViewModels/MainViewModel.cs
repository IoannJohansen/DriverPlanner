using Driver_Planner.ViewModels;
using Driver_Planner.ViewModels.Base;

namespace DriverPlanner.ViewModels
{
	internal class MainViewModel : ViewModel
	{
		private ViewModel _currentViewModel;

		public ViewModel CurrentViewModel
		{
			get 
			{ 
				return _currentViewModel; 
			}

			set 
			{ 
				Set(ref _currentViewModel, value); 
			}
		}

		public MainViewModel()
		{
			CurrentViewModel = new LoginViewModel(this);
		}

	}
}
