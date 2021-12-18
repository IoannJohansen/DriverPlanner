using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DriverPlanner.Entities;

namespace DriverPlanner.Entities
{
	[DataContract]
	[Serializable]
	[KnownType(typeof(User))]
	[KnownType(typeof(Admin))]
	[KnownType(typeof(Instructor))]
	[KnownType(typeof(BaseUser))]
	public class BaseUser
	{
		

	}
}
