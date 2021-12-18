using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DriverPlanner.Entities;

namespace DriverPlanner.Entities
{
	[DataContract]
	[Serializable]
	public class User : BaseUser
	{
		public override string ToString()
		{
			return $"FIO: {this.FIO},Email: {this.UserEMAIL}, VK: {this.UserVK}, Phone: {this.UserPhone}, Login: {this.Login}, ID: {this.UserID}, BirthDate {this.BirthDate}, Gender: {this.GenderName},";
		}

		[DataMember]
		[Required]
		[Key]
		public int UserID { get; set; }

		[DataMember]
		[Required]
		public string FIO { get; set; }

		[DataMember]
		[Required]
		public DateTime BirthDate { get; set; }

		[DataMember]
		[ForeignKey("Gender")]
		public string GenderName { get; set; }

		[Required]
		[DataMember]
		public string Login { get; set; }

		[DataMember]
		[Required]
		[EmailAddress]
		public string UserEMAIL { get; set; }

		[DataMember]
		public int ImageIndex { get; set; }

		[DataMember]
		public string UserVK { get; set; }

		[DataMember]
		[StringLength(12)]
		public string UserPhone { get; set; }

		[DataMember]
		public string HashPass { get; set; }

		[DataMember]
		public Gender Gender { get; set; }
	}
}
