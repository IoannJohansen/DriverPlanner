using Driver_Planner.Command;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class AutoParkViewModel : ViewModel
	{
		public AutoParkViewModel()
		{
			using (var dps = new DriverPlannerServiceClient())
			{
				#region Get of data
				CarList = new ObservableCollection<Cars>(dps.GetCars());
				SelectedIndex = -1;

				#endregion

				#region CommandInit
				AddCarCommand = new LambdaCommand(OnExecuteAddCarCommand, CanExecuteAddCarCommand);
				RemoveCarCommand = new LambdaCommand(OnExecuteRemoveCarCommand, CanExecuteRemoveCarCommand);

				#endregion
			}
		}

		#region Props

		private bool _listIsEnabled = true;

		public bool ListIsEnabled
		{
			get { return _listIsEnabled; }
			set { Set(ref _listIsEnabled, value); }
		}

		private ViewModel _currentVM;

		public ViewModel CurrentVM
		{
			get { return _currentVM; }
			set { Set(ref _currentVM, value); }
		}

		private Visibility _controlsVisibility = Visibility.Visible;

		public Visibility ControlsVisibility
		{
			get { return _controlsVisibility; }
			set { Set(ref _controlsVisibility, value); }
		}

		private ObservableCollection<Cars> _carList;

		public ObservableCollection<Cars> CarList
		{
			get { return _carList; }
			set 
			{ 
				Set(ref _carList, value);
				SelectedIndex = -1;
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
					_selectedCar = null;
				}
				else
				{
					Set(ref _selectedIndex, value);
					_selectedCar = CarList[value];
					CurrentVM = new CheckCarViewModel(_selectedCar);
				}
			}
		}

		private Cars _selectedCar;

		public Cars SelectedCar
		{
			get { return _selectedCar; }
			set 
			{ 
				Set(ref _selectedCar, value);
			}
		}

		#endregion

		#region Commands

		#region AddCarCommand

		public ICommand AddCarCommand { get; }
		private bool CanExecuteAddCarCommand(object p) => true;
		private void OnExecuteAddCarCommand(object p)
		{
			ListIsEnabled = false;
			ControlsVisibility = Visibility.Hidden;
			CurrentVM = new CreateCarViewModel(this);

		}
		#endregion

		#region RemoveCarcommand

		public ICommand RemoveCarCommand { get; }
		private bool CanExecuteRemoveCarCommand(object p) => SelectedIndex != -1 && SelectedCar != null;
		private void OnExecuteRemoveCarCommand(object p)
		{
			using (var dps = new DriverPlannerServiceClient())
			{
				try
				{
					dps.RemoveCar(SelectedCar.CarID);
					CarList.Remove(SelectedCar);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK);
				}
			}
		}

		#endregion

		#endregion
	}
}