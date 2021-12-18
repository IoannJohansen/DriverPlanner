using System;
using System.Runtime.Serialization;

namespace DriverPlanner.Entities
{
	[DataContract]
	public enum ERole
	{
		[EnumMember]
		USER = 1,
		[EnumMember]
		INSTRUCTOR,
		[EnumMember]
		ADMIN
	}
}
