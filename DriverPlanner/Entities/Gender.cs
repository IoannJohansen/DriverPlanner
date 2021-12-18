using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DriverPlanner.Entities
{
	[DataContract]
	public class Gender
	{
		[DataMember]
		[Key]
		[Required]
		public string GenderName { get; set; }

		[DataMember]
		[Required]
		public string GenderDescription { get; set; }

		public Gender()
		{

		}

		public Gender(string name, string descr)
		{
			this.GenderName = name;
			this.GenderDescription = descr;
		}

	}
}
