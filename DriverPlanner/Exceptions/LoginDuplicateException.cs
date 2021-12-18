using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlanner.Exceptions
{
	[DataContract]
	class LoginDuplicateException
	{
		public LoginDuplicateException(string login)
		{
			this.login = login;
		}

		[DataMember]
		public string login { get; set; }

		public override string ToString()
		{
			return $"Аккаунт с логином: {login} уже зарегестрирован";
		}
	}
}
