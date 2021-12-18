using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DriverPlanner.Entities;

namespace DriverPlanner.Entities
{
	[DataContract]
	[Serializable]
	public class Instructor : BaseUser
	{
		[Key]
		[Required]
		[DataMember]
		public int InstructorID { get; set; }
		
		[Required]
		[DataMember]
		public string FIO { get; set; }

		[DataMember]
		public int ImageIndex { get; set; }

		[ForeignKey("Gender")]
		[Required]
		[DataMember]
		public string GenderInstructor { get; set; }

		[ForeignKey("Car")]
		[Required]
		[DataMember]
		public int CarID { get; set; }

		[Required]
		[DataMember]
		public DateTime InstructorBirth { get; set; }

		[Required]
		[EmailAddress]
		[DataMember]
		public string InstructorEMAIL { get; set; }

		[DataMember]
		public string InstructorVK { get; set; }

		[DataMember]
		public string InstructorPhone { get; set; }

		[Required]
		[DataMember]
		public string Login { get; set; }

		[DataMember]
		public string HashPass { get; set; }

		[DataMember]
		public Gender Gender { get; set; }
		
		[DataMember]
		public Cars Car { get; set; }
	}
}
