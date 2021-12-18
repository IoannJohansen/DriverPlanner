using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DriverPlanner.Entities;

namespace DriverPlanner.Entities
{
	[DataContract]
	public class TimeTable
	{
		[Required]
		[Key]
		[DataMember]
		public int ClassID { get; set; }

		[Required]
		[DataMember]
		public DateTime DateOfClass { get; set; }

		[Required]
		[ForeignKey("ClassInterval")]
		[DataMember]
		public int IntervalCode { get; set; }

		[Required]
		[ForeignKey("User")]
		[DataMember]
		public int UserID { get; set; }
		
		[Required]
		[ForeignKey("Instructor")]
		[DataMember]
		public int InstructorID { get; set; }

		[DataMember]
		public User User { get; set; }

		[DataMember]
		public Instructor Instructor { get; set; }

		[DataMember]
		public ClassInterval ClassInterval { get; set; }
	}
}
