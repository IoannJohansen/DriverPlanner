using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using DriverPlanner.Data;
using DriverPlanner.Entities;
using DriverPlanner.Exceptions;

namespace DriverPlanner.Data
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DriverPlannerService" in both code and config file together.

	public class DriverPlannerService : IDisposable
	{
		/// <summary>
		/// Returns all genders
		/// </summary>
		/// <returns></returns>
		public List<Gender> GetGenders()
		{
			using (var ctx = new DriverPlanner_DB())
			{
				var cat = ctx.Genders.Select(t => t).ToList();
				return cat;
			}
		}
		
		/// <summary>
		/// Method tries to login with user data
		/// </summary>
		/// <param name="login"></param>
		/// <param name="hashPasssword"></param>
		/// <returns>(0,null) -> if invalid data, (int role, BaseUser user) -> valida data</returns>
		public (int, BaseUser) TryLogin(string login, string hashPasssword)
		{
			try
			{
				using (DriverPlanner_DB db = new DriverPlanner_DB())
				{
					#region CheckForUser
					var findedUser = db.Users.Include(t=>t.Gender).Where(t => t.Login == login && t.HashPass == hashPasssword).Select(t =>
					new
					{
						t.FIO,
						t.BirthDate,
						t.GenderName,
						t.ImageIndex,
						t.UserVK,
						t.UserEMAIL,
						t.UserPhone,
						t.UserID,
						t.Login,
						t.Gender
					}).ToList().FirstOrDefault();

					if (findedUser != null)
					{
						User user = new User()
						{
							FIO = findedUser.FIO,
							BirthDate = findedUser.BirthDate,
							GenderName = findedUser.GenderName,
							ImageIndex = findedUser.ImageIndex,
							UserVK = findedUser.UserVK,
							UserEMAIL = findedUser.UserEMAIL,
							UserPhone = findedUser.UserPhone,
							UserID = findedUser.UserID,
							Login = findedUser.Login,
							Gender = findedUser.Gender
						};
						return (1, user);
					}

					#endregion

					#region CheckForInstructor

					var findedInstructor = db.Instructors.Where(t => t.Login == login && t.HashPass == hashPasssword).Include(t=>t.Gender).Include(t=>t.Car).Select(t =>
					 new
					 {
						 FIO = t.FIO,
						 BirthDate = t.InstructorBirth,
						 GenderName = t.GenderInstructor,
						 Image = t.ImageIndex,
						 VK = t.InstructorVK,
						 EMAIL = t.InstructorEMAIL,
						 Phone = t.InstructorPhone,
						 CarID = t.CarID,
						 ID = t.InstructorID,
						 login = t.Login,
						 IGender = t.Gender,
						 Car = t.Car
					 }).ToList().FirstOrDefault();

					if (findedInstructor != null)
					{
						Instructor instructor = new Instructor()
						{
							FIO = findedInstructor.FIO,
							InstructorBirth = findedInstructor.BirthDate,
							GenderInstructor = findedInstructor.GenderName,
							ImageIndex = findedInstructor.Image,
							InstructorVK = findedInstructor.VK,
							InstructorEMAIL = findedInstructor.EMAIL,
							InstructorPhone = findedInstructor.Phone,
							CarID = findedInstructor.CarID,
							Car = findedInstructor.Car,
							InstructorID = findedInstructor.ID,
							Login = findedInstructor.login
						};
						return (2, instructor);
					}

					#endregion

					#region CheckForAdmin

					var findedAdmin = db.Admins.Where(t => t.Login == login && t.HashPass == hashPasssword).Select(t =>
					new
					{
						EMAIL = t.AdminEmail,
						ID = t.AdminID,
						login = t.Login
					}).ToList().FirstOrDefault();

					if (findedAdmin != null)
					{
						Admin admin = new Admin()
						{
							AdminEmail = findedAdmin.EMAIL,
							Name = "Admin",
							AdminID = findedAdmin.ID,
							Login = findedAdmin.login
						};
						return (3, admin);
					}
					#endregion
				}

				return (0, null);
			
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Checks for free if login and mail and fill new user data
		/// </summary>
		/// <param name="newUser"></param>
		/// <returns></returns>
		public User TryRegister(User newUser)
		{
			try
			{
				using (var ctx = new DriverPlanner_DB())
				{
					#region CheckForDuplicatesOfEmailAndLogin

					#region ForUserTable
					var resEmailInUser = ctx.Users.Where(t => t.UserEMAIL == newUser.UserEMAIL).FirstOrDefault();
					if (resEmailInUser != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(newUser.UserEMAIL), new FaultReason("Аккаунт с такой почтой уже зарегистрирован"));


					var resLoginInUser = ctx.Users.Where(t => t.Login == newUser.Login).FirstOrDefault();
					if (resLoginInUser != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(newUser.UserEMAIL), new FaultReason("Аккаунт с таким логином уже зарегистрирован"));
					#endregion

					#region ForInstructorstable
					var resEmailInIstructor = ctx.Instructors.Where(t => t.InstructorEMAIL == newUser.UserEMAIL).FirstOrDefault();
					if (resEmailInIstructor != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(newUser.UserEMAIL), new FaultReason("Аккаунт с такой почтой уже зарегистрирован"));


					var resLoginInInstructor = ctx.Instructors.Where(t => t.Login == newUser.Login).FirstOrDefault();
					if (resLoginInInstructor != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(newUser.UserEMAIL), new FaultReason("Аккаунт с таким логином уже зарегистрирован"));
					#endregion

					#region ForAdmintable
					var resEmailInAdmin = ctx.Admins.Where(t => t.AdminEmail == newUser.UserEMAIL).FirstOrDefault();
					if (resEmailInAdmin != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(newUser.UserEMAIL), new FaultReason("Аккаунт с такой почтой уже зарегистрирован"));

					var resLoginInAdmin = ctx.Admins.Where(t => t.Login == newUser.Login).FirstOrDefault();
					if (resLoginInAdmin != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(newUser.UserEMAIL), new FaultReason("Аккаунт с таким логином уже зарегистрирован"));
					#endregion

					#endregion
					var registeredUser = ctx.Users.Add(newUser);
					Console.WriteLine(registeredUser.FIO);
					ctx.SaveChanges();
					return registeredUser;
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Method checks constraints, connected with changes
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public BaseUser UpdateUser(int role, BaseUser newUserData)
		{
			using (var ctx = new DriverPlanner_DB())
			{
				BaseUser resultUser = null;
				switch (role)
				{
					case 1:
						User user = newUserData as User;
						var userInDB = ctx.Users.Where(t => t.UserID == user.UserID).FirstOrDefault();
						if (userInDB.Login != user.Login)
						{
							#region Check for free of new login
							var accWithNewLogin = ctx.Users.Where(t => t.Login == user.Login && t.UserID != user.UserID).FirstOrDefault();
							if (accWithNewLogin != null)
							{
								throw new FaultException<LoginDuplicateException>(new LoginDuplicateException(user.Login), new FaultReason("Этот логин уже занят"));
							}
							else
							{
								resultUser = RewriteUser(role, userInDB, user, ctx);
								
							}
							#endregion
						}
						else
						{
							resultUser = RewriteUser(role, userInDB, user, ctx);
						}

						break;

					case 2:

						Instructor instr = newUserData as Instructor;
						var instInDB = ctx.Instructors.Where(t => t.InstructorID == instr.InstructorID).FirstOrDefault();

						if (instInDB.Login != instr.Login)
						{
							#region Check for free of new login
							var accWithNewLogin = ctx.Instructors.Where(t => t.Login == instr.Login && t.InstructorID != instr.InstructorID).FirstOrDefault();

							if (accWithNewLogin != null)
							{
								throw new FaultException<LoginDuplicateException>(new LoginDuplicateException(instr.Login), new FaultReason("Этот логин уже занят"));
							}
							else
							{
								resultUser = RewriteUser(role, instInDB, instr, ctx);

							}
							#endregion
						}
						else
						{
							resultUser = RewriteUser(role, instInDB, instr, ctx);
						}

						break;
					default:
						break;
				}
				return resultUser;
			}
		}

		/// <summary>
		/// Method replaces the info in db for new info
		/// </summary>
		/// <param name="role">1 - coursant, 2 - instructor</param>
		/// <param name="oldUserData">User in db</param>
		/// <param name="newUserData">User which contains new info</param>
		/// <param name="ctx">DB context</param>
		public BaseUser RewriteUser(int role, BaseUser oldUserData, BaseUser newUserData, DriverPlanner_DB ctx)
		{
			switch (role)
			{
				case 1:
					User oldU = oldUserData as User;
					User newU = newUserData as User;
					

					oldU.FIO =			newU.FIO;
					oldU.BirthDate =	newU.BirthDate;
					oldU.GenderName =	newU.GenderName;
					oldU.Login =		newU.Login;
					oldU.ImageIndex = newU.ImageIndex;
					oldU.UserPhone =	newU.UserPhone;
					oldU.UserVK =		newU.UserVK;
					ctx.SaveChanges();

					return newU;
					
				case 2:         // realisation for instructor type
					Instructor oldI = oldUserData as Instructor;
					Instructor newI = newUserData as Instructor;


					oldI.FIO =				newI.FIO;
					oldI.InstructorBirth=	newI.InstructorBirth;
					oldI.GenderInstructor =	newI.GenderInstructor;
					oldI.Login =			newI.Login;
					oldI.ImageIndex =		newI.ImageIndex;
					oldI.InstructorPhone =	newI.InstructorPhone;
					oldI.InstructorVK =		newI.InstructorVK;
					oldI.CarID =			newI.CarID;
					ctx.SaveChanges();
					return newI;

				default:
					return null;
			}
			
		}

		/// <summary>
		/// Method gets dict cars with id->car name
		/// </summary>
		/// <returns></returns>
		public List<Cars> GetCars()
		{
			using (var ctx = new DriverPlanner_DB())
			{
				var cars = ctx.Cars.ToList();
				return cars;
			}
		}
		
		/// <summary>
		/// Generating the list of tasks
		/// </summary>
		/// <param name="dateClass">Date of task</param>
		/// <param name="InstructorID">Id of instructor</param>
		/// <returns></returns>
		public List<TimeTable> GetClasses(DateTime dateClass, int InstructorID)
		{
			
			List<TimeTable> defSet = new List<TimeTable>();

			#region Check for weekend

			switch (dateClass.DayOfWeek)
			{
				case DayOfWeek.Sunday:
				case DayOfWeek.Saturday:
					return defSet;
				case DayOfWeek.Monday:
					break;
				case DayOfWeek.Tuesday:
					break;
				case DayOfWeek.Wednesday:
					break;
				case DayOfWeek.Thursday:
					break;
				case DayOfWeek.Friday:
					break;
				default:
					break;
			}
			#endregion

			using (var ctx = new DriverPlanner_DB())
			{
				#region DefSet
				
				var dbSet = ctx.Timetable.Where(t => t.DateOfClass == dateClass && t.InstructorID == InstructorID).Include(t => t.Instructor.Car).Include(t=>t.ClassInterval).Include(t=>t.User.Gender).Include(t=>t.Instructor.Gender).ToList();

				for (int i = 1; i <= ctx.Intervals.Count(); i++)
				{
					bool toAdd = true;
					TimeTable tt = new TimeTable();
					tt.ClassInterval = ctx.Intervals.Find(i);
					tt.IntervalCode = i;
					tt.DateOfClass = dateClass;
					tt.InstructorID = InstructorID;
					tt.Instructor = ctx.Instructors.Where(t => t.InstructorID == InstructorID).Include(t => t.Car).Include(t=>t.Gender).FirstOrDefault();
					foreach (var item in dbSet)
					{
						if (item.ClassInterval.IntervalNumber == i)
						{
							toAdd = false;
						}
					}
					if(toAdd)defSet.Add(tt);
				}
				return defSet;
				#endregion
			}
		}

		/// <summary>
		/// Get List of all instructors
		/// </summary>
		/// <returns></returns>
		public List<Instructor> GetInstructors()
		{
			List<Instructor> res = null;
			using (var ctx = new DriverPlanner_DB())
			{
				res = ctx.Instructors.Select(t => t).Include(t=>t.Car).Include(t=>t.Gender).ToList();
				return res;
			}

		}

		/// <summary>
		/// Add new class for concrete isntructor and user on day
		/// </summary>
		/// <param name="tRow"></param>
		/// <param name="user"></param>
		/// <returns>true if done succusfully</returns>
		public bool PickTask(TimeTable tRow, User user)
		{
			try
			{
				using (var ctx = new DriverPlanner_DB())
				{
					tRow.Instructor = null;
					tRow.User = null;
					tRow.ClassInterval = null;

					ctx.Timetable.Add(tRow);
					ctx.SaveChanges();
					return true;
				}
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.InnerException);
				throw;
			}
			catch (EntityException ex)
			{
				Console.WriteLine(ex.InnerException);
				throw;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="user"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		public List<TimeTable> GetMyTasks(BaseUser user, int role)
		{
			using (var ctx = new DriverPlanner_DB())
			{
				switch (role)
				{
					case 1:
						var currentUser = user as User;
						var oldTasks = ctx.Timetable.Where(t => t.UserID == currentUser.UserID && t.DateOfClass < DateTime.Today).Include(t => t.Instructor.Car).Include(t => t.ClassInterval).ToList();

						var newTasksOfUser = ctx.Timetable.Where(t => t.UserID == currentUser.UserID && t.DateOfClass >= DateTime.Today).Include(t => t.Instructor.Car).Include(t => t.ClassInterval).OrderBy(t => t.ClassInterval.IntervalNumber).OrderBy(t => t.DateOfClass).ToList();

						var res = new List<TimeTable>(newTasksOfUser);
						res.AddRange(oldTasks);

						return res;
					
					case 2:
						var instructor = user as Instructor;

						var instructorOldTasks = ctx.Timetable.Where(t => t.InstructorID == instructor.InstructorID && t.DateOfClass < DateTime.Today).Include(t => t.ClassInterval).ToList();

						var instructorNewTasks = ctx.Timetable.Where(t => t.InstructorID == instructor.InstructorID && t.DateOfClass >= DateTime.Today).Include(t => t.User).Include(t => t.ClassInterval).ToList();

						var result = new List<TimeTable>(instructorNewTasks);
						result.AddRange(instructorOldTasks);
						return result;

					case 3:

						var nonActualTasks = ctx.Timetable.Where(t => t.DateOfClass < DateTime.Today).Include(t => t.User).Include(t => t.Instructor).Include(t => t.ClassInterval).Include(t => t.Instructor.Car).OrderBy(t => t.IntervalCode).OrderByDescending(t => t.DateOfClass).ToList();
						var actulaTask = ctx.Timetable.Where(t => t.DateOfClass >= DateTime.Today).Include(t => t.User).Include(t => t.Instructor).Include(t => t.ClassInterval).Include(t => t.Instructor.Car).OrderBy(t => t.IntervalCode).OrderBy(t => t.DateOfClass).ToList();
						var allTasks = new List<TimeTable>(actulaTask);
						allTasks.AddRange(nonActualTasks);

						return allTasks;

					default:
						return null;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="taskID"></param>
		public void CancelTask(int taskID)
		{
			using (var ctx = new DriverPlanner_DB())
			{
				var task = ctx.Timetable.Where(t => t.ClassID == taskID).FirstOrDefault();
				ctx.Timetable.Remove(task);
				ctx.SaveChanges();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public byte[] GetImage(int index)
		{
			const string prefix = @"D:\Desktop\Labs\DriverPlanner\DriverPlannerService\Resources\Images\file";
			BinaryFormatter bf = new BinaryFormatter();
			if (!File.Exists($"{prefix}{index}.dat")) return null;
			FileStream fs = new FileStream($"{prefix}{index}.dat", FileMode.Open);
			byte[] res = (byte[])bf.Deserialize(fs);
			fs.Close();
			fs.Dispose();
			return res;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="img"></param>
		/// <param name="oldIndex"></param>
		/// <returns></returns>
		public int DownloadImage(byte[] img, int oldIndex)
		{
			const string prefix = @"D:\Desktop\Labs\DriverPlanner\DriverPlannerService\Resources\Images\file";
			int index = 1;
			BinaryFormatter bf = new BinaryFormatter();
			if (!File.Exists($"{prefix}{index}.dat"))
			{
				FileStream fs = new FileStream($"{prefix}{index}.dat", FileMode.Create);
				bf.Serialize(fs, img);
				fs.Close();
				fs.Dispose();
				#region RemoveOldFile
				if (oldIndex != 0)
				{
					if (File.Exists($"{prefix}{oldIndex}.dat"))
					{
						File.Delete($"{prefix}{oldIndex}.dat");
					}

				}
				#endregion
				return index;
			}
			else
			{
				while (true)
				{
					index++;
					if (!File.Exists($"{prefix}{index}.dat"))
					{
						FileStream fs = new FileStream($"{prefix}{index}.dat", FileMode.Create);
						bf.Serialize(fs, img);
						fs.Close();
						fs.Dispose();
						#region RemoveOldFile
						if (oldIndex!=0)
						{
							if (File.Exists($"{prefix}{oldIndex}.dat"))
							{
								File.Delete($"{prefix}{oldIndex}.dat");
							}

						}
						#endregion
						return index;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public (List<Instructor> instr, List<FeedBacks> fb) GetFullProfiles()
		{
			using (var ctx = new DriverPlanner_DB())
			{
				var instructors = ctx.Instructors.Include(t => t.Car).ToList();
				var feedbacks = ctx.Feedbacks.Include(t => t.Instructor).Include(t => t.User).ToList();
				
				return (instructors, feedbacks);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newFeedback"></param>
		/// <returns></returns>
		public bool AddFeedback(FeedBacks newFeedback)
		{
			using (var ctx = new DriverPlanner_DB())
			{
				try
				{
					ctx.Feedbacks.Add(newFeedback);
					ctx.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					throw;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<FeedBacks> GetFeedbacks()
		{
			using (var ctx = new DriverPlanner_DB())
			{
				return ctx.Feedbacks.Include(t => t.User).Include(t => t.Instructor).ToList();
			}
		}

		public List<User> GetUsers()
		{
			using (var ctx =new DriverPlanner_DB())
			{
				return new List<User>(ctx.Users.ToList());
			}
		}

		public void RemoveUser(int id, int role)
		{
			using (var ctx = new DriverPlanner_DB())
			{
				switch (role)
				{
					case 1:
						User forDelete = ctx.Users.Where(t => t.UserID == id).FirstOrDefault();
						ctx.Users.Remove(forDelete);
						ctx.SaveChanges();
						break;

					case 2:
						Instructor forDelet = ctx.Instructors.Where(t => t.InstructorID == id).FirstOrDefault();
						ctx.Instructors.Remove(forDelet);
						ctx.SaveChanges();
						break;

					default:
						break;
				}
			}
		}

		public void RegisterInstrucor(Instructor instructor)
		{
			try
			{
				using (var ctx = new DriverPlanner_DB())
				{
					#region CheckForDuplicatesOfEmailAndLogin

					#region ForUserTable
					var resEmailInUser = ctx.Users.Where(t => t.UserEMAIL == instructor.InstructorEMAIL).FirstOrDefault();
					if (resEmailInUser != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(instructor.InstructorEMAIL), new FaultReason("Аккаунт с такой почтой уже зарегистрирован"));


					var resLoginInUser = ctx.Users.Where(t => t.Login == instructor.Login).FirstOrDefault();
					if (resLoginInUser != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(instructor.InstructorEMAIL), new FaultReason("Аккаунт с таким логином уже зарегистрирован"));
					#endregion

					#region ForInstructorstable
					var resEmailInIstructor = ctx.Instructors.Where(t => t.InstructorEMAIL == instructor.InstructorEMAIL).FirstOrDefault();
					if (resEmailInIstructor != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(instructor.InstructorEMAIL), new FaultReason("Аккаунт с такой почтой уже зарегистрирован"));


					var resLoginInInstructor = ctx.Instructors.Where(t => t.Login == instructor.Login).FirstOrDefault();
					if (resLoginInInstructor != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(instructor.InstructorEMAIL), new FaultReason("Аккаунт с таким логином уже зарегистрирован"));
					#endregion

					#region ForAdmintable
					var resEmailInAdmin = ctx.Admins.Where(t => t.AdminEmail == instructor.InstructorEMAIL).FirstOrDefault();
					if (resEmailInAdmin != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(instructor.InstructorEMAIL), new FaultReason("Аккаунт с такой почтой уже зарегистрирован"));

					var resLoginInAdmin = ctx.Admins.Where(t => t.Login == instructor.Login).FirstOrDefault();
					if (resLoginInAdmin != null) throw new FaultException<EmailDuplicateException>(new EmailDuplicateException(instructor.InstructorEMAIL), new FaultReason("Аккаунт с таким логином уже зарегистрирован"));
					#endregion

					#endregion

					ctx.Instructors.Add(instructor);
					ctx.SaveChanges();
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public bool CheckForTaskLimits(int userID, DateTime selectedDatetime)
		{
			using (var ctx = new DriverPlanner_DB())
			{
				try
				{
					var calendar = new GregorianCalendar();
					var tasks = new List<TimeTable>(ctx.Timetable.Where(t => t.UserID == userID).ToList());
					var startOfSelectedWeek = calendar.GetWeekOfYear(selectedDatetime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

					#region CheckForWeekLimit
					int countTasksOnWeek = 0;
					for (int i = 0; i < tasks.Count; i++)
					{
						var timeTableStartWeek = calendar.GetWeekOfYear(tasks[i].DateOfClass, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						if (startOfSelectedWeek == timeTableStartWeek) countTasksOnWeek++;
					}
					if (countTasksOnWeek >= 3) throw new FaultException<InvalidOperationException>(new InvalidOperationException(), "Превышен недельный лимит по занятиям");
					#endregion

					#region CheckForDailyLimit
					
					var dailtyTasks = ctx.Timetable.Where(t => t.DateOfClass == selectedDatetime && t.UserID == userID).ToList();
					if (dailtyTasks.Count != 0) throw new FaultException<InvalidOperationException>(new InvalidOperationException(), "Превышен дневной лимит");
					#endregion


					return true;
				}
				catch (Exception ex)
				{

					throw;
				}
			}
		}

		public List<Categories> GetCategories()
		{
			using (var ctx = new DriverPlanner_DB())
			{
				return ctx.Categories.ToList();
			}
		}

		public void AddCar(Cars newInstance)
		{
			using (var ctx = new DriverPlanner_DB())
			{
				ctx.Cars.Add(newInstance);
				ctx.SaveChanges();
			}
		}

		public void RemoveCar(int carID)
		{
			try
			{
				using (var ctx = new DriverPlanner_DB())
				{
					#region Chck for car ocupy
					var instructorsUsingThisCar = ctx.Instructors.Where(t => t.CarID == carID).ToList();
					if (instructorsUsingThisCar.Count != 0) throw new FaultException<InvalidOperationException>(new InvalidOperationException(), "Невозможно удалить т.с. используемое инструктором");
					#endregion
					var selectedCar = ctx.Cars.Where(t => t.CarID == carID).FirstOrDefault();
					ctx.Cars.Remove(selectedCar);
					ctx.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

        public void Dispose()
        {

        }
    }
}
