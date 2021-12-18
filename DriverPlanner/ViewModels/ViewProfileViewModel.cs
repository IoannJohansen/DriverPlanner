using DriverPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DriverPlanner.Command;
using DriverPlanner.Models.Classes;
using DriverPlanner.ViewModels;
using DriverPlanner.ImageConverter;
using DriverPlanner.ViewModels.Base;
using DriverPlanner.Entities;
using DriverPlanner.Data;

namespace DriverPlanner.ViewModels
{
	class ViewProfileViewModel : ViewModel
	{
		public ViewProfileViewModel()
		{

		}

		private ProfileViewModel _profileVm;

		public ProfileViewModel ProfileVm
		{
			get { return _profileVm; }
			set { Set(ref _profileVm, value); }
		}

		public ViewProfileViewModel(ProfileViewModel prof)
		{
			this._profileVm = prof;
			DriverPlannerService dps = new DriverPlannerService();
			

			switch (CurrentUserSingleton.CurrentRole)
			{
				case ERole.USER:
					#region Prop's assignment
					User curUser = CurrentUserSingleton.СurrentUser as User;
					if (curUser != null)
					{
						Fio = curUser.FIO;
						BirthDate = curUser.BirthDate;
						GenderName = curUser.GenderName;
						Login = curUser.Login;

						#region Image

						CurrentImageIndex = curUser.ImageIndex;
						Image = dps.GetImage(CurrentImageIndex);

						#endregion

						Email = curUser.UserEMAIL;
						Phone = curUser.UserPhone ?? "Номер не ввведен";
						Vk = curUser.UserVK ?? "Ссылка на вк не ввведена";
						Id = curUser.UserID;
					}

					#endregion

					CarVisibility = Visibility.Collapsed;
					EditBtnVisibilitys = Visibility.Visible;
					break;

				case ERole.INSTRUCTOR:
					#region Prop's assignment
					var carList = dps.GetCars();
					Instructor curInstructor = CurrentUserSingleton.СurrentUser as Instructor;
					CarName = carList.FirstOrDefault(t => curInstructor != null && t.CarID == curInstructor.CarID)?.CarName;
					if (curInstructor != null)
					{
						Fio = curInstructor.FIO;
						BirthDate = curInstructor.InstructorBirth;
						GenderName = curInstructor.GenderInstructor;
						Login = curInstructor.Login;

						#region Image

						CurrentImageIndex = curInstructor.ImageIndex;
						Image = dps.GetImage(CurrentImageIndex);

						#endregion

						Email = curInstructor.InstructorEMAIL;
						Phone = curInstructor.InstructorPhone ?? "Номер не ввведен";
						Vk = curInstructor.InstructorVK ?? "Ссылка на вк не ввведена";
						Id = curInstructor.InstructorID;
					}

					#endregion
					CarVisibility = Visibility.Visible;
					EditBtnVisibilitys = Visibility.Visible;
					
					break;

				case ERole.ADMIN:
					#region Prop's assignment
					Admin curAdmin = CurrentUserSingleton.СurrentUser as Admin;
					Fio = "Admin";
					BirthDate = DateTime.Today;
					GenderName = "Admin";
					if (curAdmin != null)
					{
						Login = curAdmin.Login;
						Image = DriverPlanner.Infrastructure.ImageConverter.ImageConverter.ImageToBytes(@"..\..\Resources\Images\def.png");
						Email = curAdmin.AdminEmail;
						Phone = "Admin";
						Vk = "Admin";
						Id = curAdmin.AdminID;
					}

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

		private int _presonalId;

		public int Id
		{
			get { return _presonalId; }
			set { Set(ref _presonalId, value); }
		}

		private string _fio;

		public string Fio
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

		public string Vk
		{
			get { return _vk; }
			set { Set(ref _vk, value); }
		}


		#endregion

		#region Commands

		#region ShowEditProfileMenuCommand

		public ICommand GoToEditProfileCommand { get; }
		private bool CanExecuteGoToEditProfileCommand(object p) => ProfileVm.CurrentProfileMode is ViewProfileViewModel;
		private void OnExecuteGoToEditProfileCommand(object p)
		{
			ProfileVm.CurrentProfileMode = new EditProfileViewModel(_profileVm);
		}
		#endregion

		#endregion
	}
}
