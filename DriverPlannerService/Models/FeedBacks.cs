using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlannerService.Models
{
	public class FeedBacks
	{
		[Key]
		[Required]
		public int FeedBackID { get; set; }
		
		[Required]
		public string FeedBackMessage { get; set; }

		[Required]
		[ForeignKey("User")]
		public int UserID { get; set; }

		[Required]
		[ForeignKey("Instructor")]
		public int InstructorID { get; set; }

		public Instructor Instructor { get; set; }
		public User User { get; set; }
	}
}
