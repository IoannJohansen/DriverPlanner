using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlannerService.Models
{
	[DataContract]
	[Serializable]
	public class Categories
	{
		public Categories()
		{

		}

		public Categories(string categoryName, string categoryDescription)
		{
			CategoryName = categoryName;
			CategoryDescription = categoryDescription;
		}

		[DataMember]
		[Key]
		[Required]
		[StringLength(2)]
		public string CategoryName { get; set; }
		
		[DataMember]
		[Required]
		[StringLength(100)]
		public string CategoryDescription { get; set; }

	}
}
