using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DriverPlanner.Command;
using DriverPlanner.Entities;
using DriverPlanner.ViewModels.Base;
using DriverPlanner.ViewModels;
using DriverPlanner.Entities;
using DriverPlanner.Data;

namespace DriverPlanner.ViewModels
{
	class InstructorsExplorerViewModel : ViewModel
	{
		public InstructorsExplorerViewModel()
		{
			#region Commands init
			CreateInstructorCommand = new LambdaCommand(OnExecuteCreateInstructorCommand, CanExecuteCreateInstructorCommand);
			RemoveInstructorCommand = new LambdaCommand(OnExecuteRemoveInstructorCommand, CanExecuteRemoveInstructorCommand);
			#endregion

			#region Data init

			using (var dps = new DriverPlannerService())
			{
				ListInstructors = new ObservableCollection<Instructor>(dps.GetInstructors());
				SelectedIndex = -1;

			}

			#endregion
		}

		#region Props

		private Visibility _contentVisibility;

		public Visibility ContentVisibility
		{
			get { return _contentVisibility; }
			set { Set(ref _contentVisibility, value); }
		}
		private ViewModel _currentViewModel;

		public ViewModel CurrentVM
		{
			get { return _currentViewModel; }
			set { Set(ref _currentViewModel, value); }
		}


		private ObservableCollection<Instructor> _listInstructors;

		public ObservableCollection<Instructor> ListInstructors
		{
			get { return _listInstructors; }
			set
			{ 
				Set(ref _listInstructors, value); 
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
					SelectedInstructor = null;
					ContentVisibility = Visibility.Hidden;
				}
				else
				{
					Set(ref _selectedIndex, value);
					SelectedInstructor = ListInstructors[_selectedIndex];
					ContentVisibility = Visibility.Visible;
				}
			}
		}

		private Instructor _selectedInstructor;

		public Instructor SelectedInstructor
		{
			get { return _selectedInstructor; }
			set
			{ 
				Set(ref _selectedInstructor, value);
				CurrentVM = new OverviewInstructorViewModel(_selectedInstructor);
			}
		}

		private Visibility _createButtonVisibility = Visibility.Visible;

		public Visibility ControlBtnsAreEnabled
		{
			get { return _createButtonVisibility; }
			set { Set(ref _createButtonVisibility, value); }
		}

		private bool _lsistIsEnabled = true;

		public bool ListIsEnabled
		{
			get { return _lsistIsEnabled; }
			set { Set(ref _lsistIsEnabled, value); }
		}

		#endregion


		#region Commands

		#region CreateInstructorCommand

		public ICommand CreateInstructorCommand { get; }
		private bool CanExecuteCreateInstructorCommand(object p) => true;
		private void OnExecuteCreateInstructorCommand(object p)
		{
			ControlBtnsAreEnabled = Visibility.Hidden;
			ContentVisibility = Visibility.Visible;	//!
			ListIsEnabled = false;
			CurrentVM = new CreateInstructorViewModel(this);
		}
		#endregion

		#region RemoveInstructorCommand

		public ICommand RemoveInstructorCommand { get; }
		private bool CanExecuteRemoveInstructorCommand(object p) => SelectedIndex != -1;
		private void OnExecuteRemoveInstructorCommand(object p)
		{
			using (var dps = new DriverPlannerService())
			{
				dps.RemoveUser(SelectedInstructor.InstructorID, 2);
				ListInstructors.Remove(SelectedInstructor);
			}
		}

		#endregion

		#endregion

	}
}
