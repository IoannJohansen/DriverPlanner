using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlanner.Entities
{
	[DataContract]
	[Serializable]
	public class ClassInterval
	{
		public ClassInterval(int intevalNumber, string timeInterval)
		{
			IntervalNumber = intevalNumber;
			TimeInterval = timeInterval;
		}
		
		public ClassInterval()
		{

		}

		[Key]
		[Required]
		[DataMember]
		public int IntervalNumber { get; set; }

		[Required]
		[DataMember]
		public string TimeInterval { get; set; }

	}
}
