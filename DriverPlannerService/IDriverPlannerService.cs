using DriverPlannerService.Exceptions;
using DriverPlannerService.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace DriverPlannerService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDriverPlannerService" in both code and config file together.
	[ServiceContract]
	public interface IDriverPlannerService
	{	
		/// <summary>
		/// Method for login on start
		/// </summary>
		/// <param name="login">Login</param>
		/// <param name="hashPasssword">pass after md5</param>
		/// <returns>If succsesfull -> (role, data of user) || if not finded -> (0, null)  </returns>
		[OperationContract]
		(int, BaseUser) TryLogin(string login, string hashPasssword);

		/// <summary>
		/// Validation data of new user and add user data to DB
		/// </summary>
		/// <param name="newUser">Data of new user</param>
		/// <returns>If succsesfull -> true || if not succsesfull -> false  </returns>
		[OperationContract]
		[FaultContract(typeof(EmailDuplicateException))]
		[FaultContract(typeof(LoginDuplicateException))]
		User TryRegister(User newUser);

		/// <summary>
		/// Get all genders from db
		/// </summary>
		/// <returns>list of genders</returns>
		[OperationContract]
		List<Gender> GetGenders();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[OperationContract]
		[FaultContract(typeof(LoginDuplicateException))]
		BaseUser UpdateUser(int role, BaseUser user);

		/// <summary>
		/// Replace user data from new obj to old user and commit
		/// </summary>
		/// <param name="role"></param>
		/// <param name="oldUserData"></param>
		/// <param name="newUserData"></param>
		/// <param name="ctx"></param>
		/// <returns></returns>
		BaseUser RewriteUser(int role, BaseUser oldUserData, BaseUser newUserData, DriverPlanner_DB ctx);

		/// <summary>
		/// Returns dict with (id, carName)
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		List<Cars> GetCars();

		/// <summary>
		/// Returns classes on selected date with selected instructor
		/// </summary>
		/// <param name="dateClass"></param>
		/// <returns></returns>
		[OperationContract]
		List<TimeTable> GetClasses(DateTime dateClass, int InstructorID);

		/// <summary>
		/// Returns list of all instructors
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		List<Instructor> GetInstructors();

		/// <summary>
		/// Add new class for concrete isntructor and user on day
		/// </summary>
		/// <param name="tRow"></param>
		/// <param name="user"></param>
		/// <returns>true if done succusfully</returns>
		[OperationContract]
		bool PickTask(TimeTable tRow, User user);


		[OperationContract]
		List<TimeTable> GetMyTasks(BaseUser user, int role);
		

		[OperationContract]
		void CancelTask(int taskID);


		[OperationContract]
		byte[] GetImage(int index);


		[OperationContract]
		int DownloadImage(byte[] img, int oldIndex);


		[OperationContract]
		(List<Instructor> instr, List<FeedBacks> fb) GetFullProfiles();

		[OperationContract]
		bool AddFeedback(FeedBacks newFeedback);

		[OperationContract]
		List<FeedBacks> GetFeedbacks();

		[OperationContract]
		List<User> GetUsers();

		[OperationContract]
		void RemoveUser(int id, int role);

		[OperationContract]
		void RegisterInstrucor(Instructor instructor);

		[OperationContract]
		bool CheckForTaskLimits(int userID, DateTime selectedDatetime);

		[OperationContract]
		List<Categories> GetCategories();

		[OperationContract]
		void AddCar(Cars newInstance);

		[OperationContract]
		void RemoveCar(int carID);
	}
}