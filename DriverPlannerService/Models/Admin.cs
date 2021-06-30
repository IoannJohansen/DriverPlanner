﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DriverPlannerService.Models
{
	[DataContract]
	[Serializable]
	public class Admin : BaseUser
	{
		[Key]
		[Required]
		[DataMember]
		public int AdminID { get; set; }

		[Required]
		[DataMember]
		public string Login { get; set; }

		[Required]
		[DataMember]
		public string Name { get; set; }
		
		[Required]
		[DataMember]
		public string AdminEmail { get; set; }
		
		[Required]
		[DataMember]
		public string HashPass { get; set; }
	}
}
