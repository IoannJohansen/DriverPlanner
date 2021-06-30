using Driver_Planner.Command;
using Driver_Planner.Hash;
using Driver_Planner.Model;
using Driver_Planner.Models;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using DriverPlanner.Infrastructure.Attribute;
using DriverPlanner.MyValidator;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class LoginViewModel : ViewModel
	{
		private MainViewModel MainVM { get; }
		
		public LoginViewModel()
		{
			
		}

		public LoginViewModel(MainViewModel mainVM)
		{
			this.MainVM = mainVM;

			#region Command's init
			UpdateViewCommand = new UpdateViewCommand(mainVM);
			ConfirmEntryCommand = new LambdaCommand(OnExecuteConfirmEntryCommand, CanExecuteConfirmEntryCommand);
			

			#endregion


		}

		#region Props

		private string _login;
		[Required(ErrorMessage = "Лоигн обязателен для заполнения")]
		[StringLength(12, ErrorMessage = "Максимальная длинна логина: 12"), MinLength(6, ErrorMessage = "Минимальная длинна логина: 6")]
		[Login]
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
		[StringLength(12, ErrorMessage = "Допустимая длинна пароля: от 6 до 12 символов"), MinLength(6, ErrorMessage = "Минимальная длинна пароля: 6")]
		[Password]
		public string Password
		{
			get { return _password; }
			set { Set(ref _password, value); }
		}

		#endregion

		#region Commands
		public ICommand UpdateViewCommand { get; }
		
		#region ConfirmLoginCommand
		public ICommand ConfirmEntryCommand { get; }
		private bool CanExecuteConfirmEntryCommand(object p) => true;
		private void OnExecuteConfirmEntryCommand(object p)
		{
			string error = MyValidator.Validate(this);
			if (error!=null)
			{
				ErrorMSG = error;
			}
			else
			{
				DriverPlannerServiceClient dps = new DriverPlannerServiceClient();
				var hashPass = HashPassword.GetHash(Password);
				var user = dps.TryLogin(Login, hashPass);
				if (user.Item1 != 0)
				{
					CurrentUserSingleton.СurrentUser = user.Item2;
					switch (user.Item1)
					{
						case 1:
							CurrentUserSingleton.CurrentRole = ERole.USER;
							break;
						case 2:
							CurrentUserSingleton.CurrentRole = ERole.INSTRUCTOR;
							break;
						case 3:
							CurrentUserSingleton.CurrentRole = ERole.ADMIN;
							break;
						default:
							break;
					}
					UpdateViewCommand.Execute(p);
				}
				else
				{
					ErrorMSG = "Проверьте входные данные на правильность";
				}
			}
		}

		#endregion

		#endregion

	}
}
