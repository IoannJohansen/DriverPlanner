using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DriverPlanner.Infrastructure.Attribute
{
	public class PhoneNumber : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			string inp = value as string;
			if (String.IsNullOrEmpty(inp)) return true;
			Regex regex = new Regex(@"^375((17|29|33|44))[0-9]{3}[0-9]{2}[0-9]{2}$");
			if (regex.IsMatch(inp))
				return true;
			else
				this.ErrorMessage = "Неверный формат номера телефона";
			return false;
		}
	}
}
