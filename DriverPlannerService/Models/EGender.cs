using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlannerService.Models
{
	[DataContract]
	[Serializable]
	public enum EGender
	{
		[EnumMember]
		male = 1,
		[EnumMember]
		female
	}
}
