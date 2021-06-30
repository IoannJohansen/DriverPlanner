using Driver_Planner.Command;
using Driver_Planner.Model;
using Driver_Planner.Models;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using DriverPlanner.Infrastructure.ImageConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Driver_Planner.ViewModels
{
	class ViewProfileViewModel : ViewModel
	{
		public ViewProfileViewModel()
		{

		}

		private ProfileViewModel _profileVM;

		public ProfileViewModel ProfileVM
		{
			get { return _profileVM; }
			set { Set(ref _profileVM, value); }
		}

		public ViewProfileViewModel(ProfileViewModel prof)
		{
			this._profileVM = prof;
			DriverPlannerServiceClient dps = new DriverPlannerServiceClient();
			

			switch (CurrentUserSingleton.CurrentRole)
			{
				case ERole.USER:
					#region Prop's assignment
					User curUser = CurrentUserSingleton.СurrentUser as User;
					FIO = curUser.FIO;
					BirthDate = curUser.BirthDate;
					GenderName = curUser.GenderName;
					Login = curUser.Login;

					#region Image
					CurrentImageIndex = curUser.ImageIndex;
					Image = dps.GetImage(CurrentImageIndex);
					#endregion

					Email = curUser.UserEMAIL;
					Phone = curUser.UserPhone != null ? curUser.UserPhone : "Номер не ввведен";
					VK = curUser.UserVK != null ? curUser.UserVK : "Ссылка на вк не ввведена";
					ID = curUser.UserID;
					#endregion

					CarVisibility = Visibility.Collapsed;
					EditBtnVisibilitys = Visibility.Visible;
					break;

				case ERole.INSTRUCTOR:
					#region Prop's assignment
					var carList = dps.GetCars();
					Instructor curInstructor = CurrentUserSingleton.СurrentUser as Instructor;
					CarName = carList.Where(t => t.CarID == curInstructor.CarID).FirstOrDefault().CarName;
					FIO = curInstructor.FIO;
					BirthDate = curInstructor.InstructorBirth;
					GenderName = curInstructor.GenderInstructor;
					Login = curInstructor.Login;
					#region Image

					CurrentImageIndex = curInstructor.ImageIndex;
					Image = dps.GetImage(CurrentImageIndex);
					
					#endregion
					Email = curInstructor.InstructorEMAIL;
					Phone = curInstructor.InstructorPhone != null ? curInstructor.InstructorPhone : "Номер не ввведен";
					VK = curInstructor.InstructorVK != null ? curInstructor.InstructorVK : "Ссылка на вк не ввведена";
					ID = curInstructor.InstructorID;
					#endregion
					CarVisibility = Visibility.Visible;
					EditBtnVisibilitys = Visibility.Visible;
					
					break;

				case ERole.ADMIN:
					#region Prop's assignment
					Admin curAdmin = CurrentUserSingleton.СurrentUser as Admin;
					FIO = "Admin";
					BirthDate = DateTime.Today;
					GenderName = "Admin";
					Login = curAdmin.Login;
					Image = ImageConverter.ImageToBytes(@"..\..\Resources\Images\def.png");
					Email = curAdmin.AdminEmail;
					Phone = "Admin";
					VK = "Admin";
					ID = curAdmin.AdminID;
					#endregion
					CarVisibility = Visibility.Collapsed;
					EditBtnVisibilitys = Visibility.Collapsed;
					break;

				default:
					break;
			}

			#region CommandsInit
			GoToEditProfileCommand = new LambdaCommand(OnExecuteGoToEditProfileCommand, CanExecuteGoToEditProfileCommand);

			#endregion
		}

		#region Props

		private int _currentImageIndex;

		public int CurrentImageIndex
		{
			get { return _currentImageIndex; }
			set { Set(ref _currentImageIndex, value); }
		}

		private List<Cars> _listCars;

		public List<Cars> ListCars
		{
			get { return _listCars; }
			set { Set(ref _listCars, value); }
		}

		private string _carName;

		public string CarName
		{
			get { return _carName; }
			set { Set(ref _carName, value); }
		}

		private Visibility _editButtonInabled;

		public Visibility EditBtnVisibilitys
		{
			get { return _editButtonInabled; }
			set { Set(ref _editButtonInabled, value); }
		}

		private Visibility _carVisibility;

		public Visibility CarVisibility
		{
			get { return _carVisibility; }
			set { Set(ref _carVisibility, value); }
		}

		private int _presonalID;

		public int ID
		{
			get { return _presonalID; }
			set { Set(ref _presonalID, value); }
		}

		private string _fio;

		public string FIO
		{
			get { return _fio; }
			set { Set(ref _fio, value); }
		}

		private DateTime _birthDate;

		public DateTime BirthDate
		{
			get { return _birthDate; }
			set { Set(ref _birthDate, value); }
		}

		private string _genderName;

		public string GenderName
		{
			get { return _genderName; }
			set { Set(ref _genderName, value); }
		}

		private string _login;

		public string Login
		{
			get { return _login; }
			set { Set(ref _login, value); }
		}

		private byte[] _image;

		public byte[] Image
		{
			get { return _image; }
			set { Set(ref _image, value); }
		}

		private string _email;

		public string Email
		{
			get { return _email; }
			set { Set(ref _email, value); }
		}

		private string _phone;

		public string Phone
		{
			get { return _phone; }
			set { Set(ref _phone, value); }
		}

		private string _vk;

		public string VK
		{
			get { return _vk; }
			set { Set(ref _vk, value); }
		}


		#endregion

		#region Commands

		#region ShowEditProfileMenuCommand

		public ICommand GoToEditProfileCommand { get; }
		private bool CanExecuteGoToEditProfileCommand(object p) => ProfileVM.CurrentProfileMode is ViewProfileViewModel;
		private void OnExecuteGoToEditProfileCommand(object p)
		{
			ProfileVM.CurrentProfileMode = new EditProfileViewModel(_profileVM);
		}
		#endregion

		#endregion
	}
}
