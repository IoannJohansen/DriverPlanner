using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.Command;
using DriverPlanner.DPService;

namespace DriverPlanner.ViewModels
{
	class UsersExplorerViewModel : ViewModel
	{
		public UsersExplorerViewModel()
		{
			using (var dps = new DriverPlannerServiceClient())
			{
				RemoveUserCommand = new LambdaCommand(OnExecuteRemoveUserCommand, CanExecuteRemoveUserCommand);

				UserList = new ObservableCollection<User>(dps.GetUsers());
				SelectedIndex = -1;
			
			}
		}



		#region Props

		private Visibility _contentVisibility;

		public Visibility ContentVisibility
		{
			get { return _contentVisibility; }
			set { Set(ref _contentVisibility, value); }
		}

		private ObservableCollection<User> _userList;

		public ObservableCollection<User> UserList
		{
			get { return _userList; }
			set 
			{ 
				Set(ref _userList, value); 
			}
		}

		private int _selectedIndex;

		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				if (value<0)
				{
					Set(ref _selectedIndex, value);
					CurrentUser = null;
					ContentVisibility = Visibility.Hidden;
				}
				else
				{
					Set(ref _selectedIndex, value); 
					CurrentUser = UserList[_selectedIndex];
					ContentVisibility = Visibility.Visible;
				}
			}
		}

		private User _currentUser;

		public User CurrentUser
		{
			get { return _currentUser; }
			set { Set(ref _currentUser, value); }
		}

		#endregion

		#region Commands

		#region RemoveUserCommand
		public ICommand RemoveUserCommand { get; }
		private bool CanExecuteRemoveUserCommand(object p) => CurrentUser != null;
		private void OnExecuteRemoveUserCommand(object p)
		{
			using (var dps = new DriverPlannerServiceClient())
			{
				dps.RemoveUser(_currentUser.UserID, 1);
				UserList.Remove(_currentUser);
			}
		}
		#endregion

		#endregion

	}
}
