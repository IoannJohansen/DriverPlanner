﻿using Driver_Planner.Command;
using Driver_Planner.Models;
using Driver_Planner.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class WorkingViewModel : ViewModel
	{
		private ViewModel _currentVM;

		public ViewModel CurrentVM
		{
			get => _currentVM;
			set => Set(ref _currentVM, value);
		}

		private MainViewModel MainVM { get; }

		public WorkingViewModel(MainViewModel mVM)
		{
			this.MainVM = mVM;
			#region CommandsInit
			UpdateViewCommand = new LambdaCommand(OnExecuteUpdateViewCommand, CanExecuteUpdateViewCommand);
			GoToStart = new LambdaCommand(OnExecuteGoToStart, CanExecuteGoToStart);
			#endregion
			switch (CurrentUserSingleton.CurrentRole)
			{
				case Model.ERole.USER:
					AdminLabelVisibility = Visibility.Collapsed;
					break;
				case Model.ERole.INSTRUCTOR:
					AdminLabelVisibility = Visibility.Collapsed;
					break;
				case Model.ERole.ADMIN:
					AdminLabelVisibility = Visibility.Visible;
					break;
				default:
					break;
			}
		}

		#region Props

		private Visibility _adminLabelVisibility = Visibility.Collapsed;

		public Visibility AdminLabelVisibility
		{
			get { return _adminLabelVisibility; }
			set { Set(ref _adminLabelVisibility, value); }
		}

		#endregion

		#region Commands

		#region GotoTheProfileVM
		public ICommand UpdateViewCommand { get; }
		private bool CanExecuteUpdateViewCommand(object p) => true;
		private void OnExecuteUpdateViewCommand(object p)
		{
			if ((string)p == "Profile")
			{
				CurrentVM = new ProfileViewModel();
			}
			else if ((string)p == "Table")
			{
				CurrentVM = new TimeTableViewModel();

			}
			else if ((string)p == "Tasks")
			{
				switch (CurrentUserSingleton.CurrentRole)
				{
					case Model.ERole.USER:
						CurrentVM = new MyTasksViewModel();
						break;
					case Model.ERole.INSTRUCTOR:
						CurrentVM = new InstructorTaskViewModel();
						break;
					case Model.ERole.ADMIN:
						CurrentVM = new AdminTasksViewModel();
						break;
					default:
						break;
				}
			}
			else if ((string)p == "Instructors")
			{
				CurrentVM = new ViewInstructorsViewModel();
			}
			else if ((string)p == "UserExp")
			{
				CurrentVM = new UsersExplorerViewModel();
			}
			else if ((string)p == "AutoPark")
			{
				CurrentVM = new AutoParkViewModel();
			}
			else if ((string)p == "InstructorsExp")
			{
				CurrentVM = new InstructorsExplorerViewModel();

			}


		}
		#endregion

		#region GoBackToLogReg

		public ICommand GoToStart { get; }
		private bool CanExecuteGoToStart(object p) => true;
		private void OnExecuteGoToStart(object p)
		{
			MainVM.CurrentViewModel = new LoginViewModel(MainVM);
			CurrentUserSingleton.СurrentUser = null;
			
		}

		#endregion

		#endregion

	}
}
