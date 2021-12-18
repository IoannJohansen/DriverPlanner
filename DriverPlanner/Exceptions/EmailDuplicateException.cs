using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlanner.Exceptions
{
	[DataContract]
	public class EmailDuplicateException
	{
		public EmailDuplicateException(string email)
		{
			EMAIL = email;
		}

		[DataMember]
		public string EMAIL { get; set; }

		public override string ToString()
		{
			return $"Аккаунт с почтой: {EMAIL} уже зарегестрирован";
		}

	}
}
