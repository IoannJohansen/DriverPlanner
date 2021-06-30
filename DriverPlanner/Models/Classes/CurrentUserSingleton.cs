using Driver_Planner.Model;
using DriverPlanner.DPService;

namespace Driver_Planner.Models
{
	class CurrentUserSingleton
	{
		private static ERole _currentRole;
		public static ERole CurrentRole
		{
			get
			{
				return _currentRole;
			}
			set
			{
				_currentRole = value;
			}
		}

		public static BaseUser СurrentUser { get; set; }

		private static readonly CurrentUserSingleton _userSingleton;

		public CurrentUserSingleton GetCurrentUserSingleton() => _userSingleton;

		static CurrentUserSingleton() => _userSingleton = new CurrentUserSingleton();

		private CurrentUserSingleton() { }
	}
}
