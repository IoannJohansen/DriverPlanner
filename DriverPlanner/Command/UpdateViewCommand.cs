using System;
using System.Windows.Input;
using DriverPlanner.ViewModels;

namespace DriverPlanner.Command
{
	class UpdateViewCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private MainViewModel _viewModel;

		public UpdateViewCommand(MainViewModel viewModel)
		{
			this._viewModel = viewModel;
		}

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter)
		{
			if (parameter.ToString() == "Login")
			{
				_viewModel.CurrentViewModel = new LoginViewModel(_viewModel);
			}
			else if (parameter.ToString() == "Register")
			{
				_viewModel.CurrentViewModel = new RegisterViewModel(_viewModel);
			}
			else if (parameter.ToString() == "WorkingSpace")
			{
				_viewModel.CurrentViewModel = new WorkingViewModel(_viewModel);
			}
			

		}
	}
}
