using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using DriverPlanner.Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DriverPlanner.Command;
using DriverPlanner.Infrastructure.Hash;
using DriverPlanner.Infrastructure.Validator;
using DriverPlanner.ViewModels;

namespace Driver_Planner.ViewModels
{
	class CreateInstructorViewModel : ViewModel
	{
		public CreateInstructorViewModel(InstructorsExplorerViewModel parrentVm)
		{
			this.ParrentVM = parrentVm;
			NewInstance = new Instructor();
			using (var dps = new DriverPlannerServiceClient())
			{
				CarList = new List<Cars>(dps.GetCars());
				GenderList = new List<Gender>(dps.GetGenders());

			}
			ConfirmCreateCommand = new LambdaCommand(OnExecuteConfirmCreateCommand, CanExecuteConfirmCreateCommand);
			CancelCreation = new LambdaCommand(OnExecuteCancelCreation, CanExecuteCancelCreation);
		}

		public CreateInstructorViewModel()
		{

		}

		#region Props

		private Instructor _newInstance;

		public Instructor NewInstance
		{
			get { return _newInstance; }
			set { Set(ref _newInstance, value); }
		}

		public InstructorsExplorerViewModel ParrentVM { get; set; }

		private int _indexImage;

		public int CurrentImageIndex
		{
			get { return _indexImage; }
			set { _indexImage = value; }
		}

		private int _selectedCar;

		public int SelectedCar
		{
			get { return _selectedCar; }
			set { Set(ref _selectedCar, value); }
		}

		private List<Cars> _carList;

		public List<Cars> CarList
		{
			get { return _carList; }
			set { Set(ref _carList, value); }
		}


		private string _carName;

		public string CarName
		{
			get { return _carName; }
			set { Set(ref _carName, value); }
		}

		public int ID { get; set; }

		private byte[] _userImage;

		public byte[] UserImage
		{
			get { return _userImage; }
			set { Set(ref _userImage, value); }
		}

		private string _mail;
		[EmailAddress]
		public string Mail
		{
			get { return _mail; }
			set { Set(ref _mail, value); }
		}

		private string _errorMSG;

		public string ErrorMSG
		{
			get { return _errorMSG; }
			set { Set(ref _errorMSG, value); }
		}

		public List<Gender> GenderList { get; set; }

		private ProfileViewModel _profileVM;

		public ProfileViewModel Profile
		{
			get { return _profileVM; }
			set { Set(ref _profileVM, value); }
		}

		private string _fio;
		[Required(ErrorMessage = "ФИО обязательно для заполнения")]
		[FIo]
		public string FIO
		{
			get { return _fio; }
			set { Set(ref _fio, value); }
		}

		private DateTime _birthDate = DateTime.Today;
		
		[BirthDateAtribute]
		public DateTime BirthDate
		{
			get { return _birthDate; }
			set { Set(ref _birthDate, value); }
		}


		private int _selectedGender;

		public int SelectedGender
		{
			get { return _selectedGender; }
			set { Set(ref _selectedGender, value); }
		}


		private string _login;
		[Required(ErrorMessage = "Логин обязателен для заполнения")]
		[Login]
		public string Login
		{
			get { return _login; }
			set { Set(ref _login, value); }
		}

		private string _password;

		[Required(ErrorMessage = "Пароль не введен")]
		[Password]
		public string Password
		{
			get { return _password; }
			set { Set(ref _password, value); }
		}


		#endregion

		#region Commands

		#region ConfirmCreateCommands
		public ICommand ConfirmCreateCommand { get; }
		private bool CanExecuteConfirmCreateCommand(object p) => true;
		private void OnExecuteConfirmCreateCommand(object p)
		{
			try
			{
				#region Fill the new instance
				
				_newInstance.FIO = _fio;
				_newInstance.HashPass = HashPassword.GetHash(_password);
				_newInstance.InstructorBirth = _birthDate;
				_newInstance.Login = _login;
				_newInstance.GenderInstructor = GenderList[SelectedGender].GenderName;
				_newInstance.CarID = CarList[SelectedCar].CarID;
				_newInstance.InstructorEMAIL = Mail;
				_newInstance.ImageIndex = 0;

				#endregion
				var errorMsg = MyValidator.Validate(this);

				if (errorMsg != null)
				{
					ErrorMSG = errorMsg;
				}
				else
				{
					using (var dps = new DriverPlannerServiceClient())
					{
						dps.RegisterInstrucor(_newInstance);
						ParrentVM.ListInstructors = new ObservableCollection<Instructor>(dps.GetInstructors());
						ParrentVM.CurrentVM = new OverviewInstructorViewModel();
						ParrentVM.ListIsEnabled = true;
						ParrentVM.ControlBtnsAreEnabled = System.Windows.Visibility.Visible;
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

		}
		#endregion

		#region CancelCreateCommand

		public ICommand CancelCreation { get; }
		private bool CanExecuteCancelCreation(object p) => true;
		private void OnExecuteCancelCreation(object p)
		{
			ParrentVM.CurrentVM = new OverviewInstructorViewModel();
			ParrentVM.ListIsEnabled = true;
			ParrentVM.ControlBtnsAreEnabled = System.Windows.Visibility.Visible;
		}
		#endregion

		#endregion

	}
}
