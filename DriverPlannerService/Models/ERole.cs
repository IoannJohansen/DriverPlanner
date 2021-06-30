using System;
using System.Runtime.Serialization;

namespace DriverPlannerService.Models
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
