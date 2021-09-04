using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DriverPlanner.Infrastructure.Attribute
{
	public class LoginAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			string inp = value as string;
			Regex regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9_]*$");
			if (inp != null && regex.IsMatch(inp))
				return true;
			else
				this.ErrorMessage = "Логин содержит только латиницу(оба регистра) и цифры";
			return false;
		}
	}
}
