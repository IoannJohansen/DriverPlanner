using Driver_Planner.Command;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using DriverPlanner.Infrastructure.FileDialog;
using DriverPlanner.Infrastructure.ImageConverter;
using DriverPlanner.MyValidator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class CreateCarViewModel : ViewModel
	{
		public CreateCarViewModel(AutoParkViewModel parrentVM)
		{
			this.ParrentVM = parrentVM;
			this._newInstance = new Cars();

			using (var dps = new DriverPlannerServiceClient())
			{
				this.Categories = new ObservableCollection<Categories>(dps.GetCategories());

			}

			#region Commands init
			ConfirmCommand = new LambdaCommand(OnExecuteConfirmCommand, CanExecuteConfirmCommand);
			ChangeImageCommand = new LambdaCommand(OnExecuteChangeImageCommand, CanExecuteChangeImageCommand);
			CancelCreationCommand = new LambdaCommand(OnExecuteCancelCreationCommand, CanExecuteCancelCreationCommand);


			#endregion


		}

		public CreateCarViewModel()
		{

		}

		#region Props

		private string _errorMSG;

		public string ErrorMSG
		{
			get { return _errorMSG; }
			set { Set(ref _errorMSG, value); }
		}

		public AutoParkViewModel ParrentVM { get; set; }

		private ObservableCollection<Categories> _categories;

		public ObservableCollection<Categories> Categories
		{
			get { return _categories; }
			set { Set(ref _categories, value); }
		}

		private Cars _newInstance;

		public Cars NewInstance
		{
			get { return _newInstance; }
			set { Set(ref _newInstance, value); }
		}

		private string _carName;

		[Required(ErrorMessage = "Не введено название авто")]
		[MinLength(5, ErrorMessage = "Минимальная длинна названия - 5 символов")]
		[MaxLength(20, ErrorMessage = "Максимальная длинна названия - 20 символов")]
		public string CarName
		{
			get { return _carName; }
			set { Set(ref _carName, value); }
		}

		private int _year = DateTime.Today.Year;

		public int Year
		{
			get { return _year; }
			set { Set(ref _year, value); }
		}

		private int _imageIndex;

		public int ImageIndex
		{
			get { return _imageIndex; }
			set { Set(ref _imageIndex, value); }
		}

		private int _selectedCategorie;

		public int SelectedCategorie
		{
			get { return _selectedCategorie; }
			set { Set(ref _selectedCategorie, value); }
		}
		#endregion

		#region Commands

		#region ConfirmCommand

		public ICommand ConfirmCommand { get; }
		private bool CanExecuteConfirmCommand(object p) => true;
		private void OnExecuteConfirmCommand(object p)
		{
			#region fill new instance & validation
			_newInstance.CarName = _carName;
			_newInstance.CarYear = _year;
			_newInstance.CategoryName = Categories[_selectedCategorie].CategoryName;
			_newInstance.ImageIndex = _imageIndex;
			#endregion

			string errors = MyValidator.Validate(this);
			
			if (errors!=null)
			{
				ErrorMSG = errors;
			}
			else
			{
				using (var dps = new DriverPlannerServiceClient())
				{
					dps.AddCar(_newInstance);
					ParrentVM.CarList = new ObservableCollection<Cars>(dps.GetCars());
				}
				ParrentVM.CurrentVM = new CheckCarViewModel();
				ParrentVM.ListIsEnabled = true;
				ParrentVM.ControlsVisibility = System.Windows.Visibility.Visible;
			}
		}
		#endregion

		#region ChangeImageCommand
		public ICommand ChangeImageCommand { get; }
		private bool CanExecuteChangeImageCommand(object p) => true;
		private void OnExecuteChangeImageCommand(object p)
		{
			var path = FileSelectorDialog.FileDiaolog();
			if (path == null) return;
			var image = ImageConverter.ImageToBytes(path);

			using (var dps = new DriverPlannerServiceClient())
			{
				int newIndex = dps.DownloadImage(image, 0);
				ImageIndex = newIndex;
			}
		}
		#endregion

		#region CancelCreationCommand
		public ICommand CancelCreationCommand { get; }
		private bool CanExecuteCancelCreationCommand(object p) => true;
		private void OnExecuteCancelCreationCommand(object p)
		{
			ParrentVM.CurrentVM = new CheckCarViewModel();
			ParrentVM.ListIsEnabled = true;
			ParrentVM.ControlsVisibility = System.Windows.Visibility.Visible;
		}
		#endregion

		#endregion
	}
}
