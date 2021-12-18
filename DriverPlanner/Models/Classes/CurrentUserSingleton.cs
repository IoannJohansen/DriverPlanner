using DriverPlanner.Entities;

namespace DriverPlanner.Models.Classes
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

		private static readonly CurrentUserSingleton UserSingleton;

		public CurrentUserSingleton GetCurrentUserSingleton() => UserSingleton;

		static CurrentUserSingleton() => UserSingleton = new CurrentUserSingleton();

		private CurrentUserSingleton() { }
	}
}
