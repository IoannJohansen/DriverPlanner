using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DriverPlannerService.Models
{
	[DataContract]
	[Serializable]
	public class Cars
	{

		public Cars()
		{
		}

		public Cars(string carName, int carYear, Categories category)
		{
			CarName = carName;
			CarYear = carYear;
			Category = category;
			ImageIndex = 0;
		}



		[DataMember]
		[Key]
		[Required]
		public int CarID { get; set; }

		[DataMember]
		[Required]
		[StringLength(20)]
		public string CarName { get; set; }

		[DataMember]
		public int ImageIndex { get; set; }

		[DataMember]
		[ForeignKey("Category")]
		[StringLength(2)]
		[Required]
		public string CategoryName { get; set; }
		
		[DataMember]
		[Required]
		[Range(1945, 2021)]
		public int CarYear { get; set; }
		
		[DataMember]
		public Categories Category { get; set; }
	}
}
