using Driver_Planner.Command;
using Driver_Planner.Hash;
using Driver_Planner.Model;
using Driver_Planner.Models;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using DriverPlanner.Infrastructure.Attribute;
using DriverPlanner.Infrastructure.Converters;
using DriverPlanner.Models.Enums;
using DriverPlanner.MyValidator;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class RegisterViewModel : ViewModel
	{
		private MainViewModel MainVM { get; }

		public ObservableCollection<Gender> Genders { get; }

		public RegisterViewModel()
		{

		}

		public RegisterViewModel(MainViewModel mainVM)
		{
			this.MainVM = mainVM;
			UpdateViewCommand = new UpdateViewCommand(mainVM);

			DriverPlannerServiceClient dps = new DriverPlannerServiceClient();
			Genders = new ObservableCollection<Gender>(dps.GetGenders());

			#region Command's init

			ConfirmRegisterCommand = new LambdaCommand(OnExecuteConfirmRegisterCommand, CanExecuteConfirmRegisterCommand);

			#endregion

		}

		#region Props

		private string _fio;
		[Required(ErrorMessage = "ФИО обязательно для заполнения")]
		[FIo]
		[MinLength(6, ErrorMessage = "Минимальная длинна ФИО - 6"), MaxLength(30, ErrorMessage = "Максимальная длинна ФИО - 30")]
		public string FIO
		{
			get { return _fio; }
			set { Set(ref _fio, value); }
		}

		private string _login;
		[Required(ErrorMessage = "Логин обязателен для заполнения")]
		//[RegularExpression(@"", ErrorMessage = "Неверный формат логина")]
		[Login]
		[MinLength(6, ErrorMessage = "Минимальная длинна логина - 6"), MaxLength(12, ErrorMessage = "Максимальная длинна логина - 12")]
		public string Login
		{
			get { return _login; }
			set { Set(ref _login, value); }
		}

		private string _errorMsg = "";

		public string ErrorMSG
		{
			get { return _errorMsg; }
			set { Set(ref _errorMsg, value); }
		}

		private string _password;
		[Required(ErrorMessage = "Пароль обязателен для заполнения")]
		[StringLength(12, ErrorMessage = "Максимальная длинна пароля: 12 символов"), MinLength(6, ErrorMessage = "Минимальная длинна пароля: 6")]
		[Password]
		public string Password
		{
			get { return _password; }
			set { Set(ref _password, value); }
		}

		private string _confirmedPassword;
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		[Password]
		[Required(ErrorMessage = "Подтвердите пароль")]
		public string ConfirmedPassword
		{
			get { return _confirmedPassword; }
			set { Set(ref _confirmedPassword, value); }
		}

		private EGender _selectedgender;

		public int SelectedGender
		{
			get { return (int)_selectedgender; }
			set { Set(ref _selectedgender, (EGender)value); }
		}


		private DateTime _birthDate = DateTime.Today;
		[BirthDateAtribute]
		public DateTime BirthDate
		{
			get { return _birthDate; }
			set { Set(ref _birthDate, value); }
		}

		private string _email;
		[Required(ErrorMessage = "Требуется ввод почты")]
		[EmailAddress(ErrorMessage = "Неверный формат почты")]
		
		public string Email
		{
			get { return _email; }
			set { Set(ref _email, value); }
		}

		#endregion

		#region Command's

		public ICommand UpdateViewCommand { get; }

		#region ConfirmRegisterCommand

		public ICommand ConfirmRegisterCommand { get; }
		private bool CanExecuteConfirmRegisterCommand(object p) => true;
		private void OnExecuteConfirmRegisterCommand(object p)
		{
			string error = MyValidator.Validate(this);
			if (error == null)
			{
				try
				{
					User user = null;
					DriverPlannerServiceClient dps = new DriverPlannerServiceClient();
					user = new User();
					user.FIO = _fio;
					user.BirthDate = _birthDate;
					user.GenderName = Genders[SelectedGender].GenderName;
					user.Login = _login;
					user.UserEMAIL = _email;
					user.HashPass = HashPassword.GetHash(_password);
					

					User registeredUser = null;

					if ((registeredUser =  dps.TryRegister(user)) != null)
					{
						CurrentUserSingleton.СurrentUser = registeredUser;
						CurrentUserSingleton.CurrentRole = ERole.USER;
						UpdateViewCommand.Execute(p);
					}
					
				}
				catch (FaultException<EmailDuplicateException> ex)
				{
					ErrorMSG = ex.Message;
				}
				catch(FaultException<LoginDuplicateException> ex)
				{
					ErrorMSG = ex.Message;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					throw;
				}
			}
			else
			{
				ErrorMSG = error;
			}

		}
		#endregion

		#endregion
	}
}
