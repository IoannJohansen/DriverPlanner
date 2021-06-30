using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DriverPlanner.Infrastructure.Attribute
{
	public class Password : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			string inp = value as string;
			if (inp == null) return false;
			Regex regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$");
			if (regex.IsMatch(inp))
				return true;
			else
				this.ErrorMessage = "Пароль: буквы и цифры";
			return false;
		}
	}
}
