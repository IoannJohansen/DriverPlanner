using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverPlanner.MyValidator
{
	public static class MyValidator	
	{
		public static string Validate(dynamic obj)
		{
			var results = new List<ValidationResult>();
			var context = new ValidationContext(obj);

			if (!Validator.TryValidateObject(obj, context, results, true))
			{
				foreach (var error in results)
				{
					return error.ErrorMessage;
				}
			}
			return null;
		}
	}
}
