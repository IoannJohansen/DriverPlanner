using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DriverPlanner.Infrastructure.Attribute
{
	public class FIoAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			string inp = value as string;
			Regex regex = new Regex(@"(^[А-Яа-яёЁ]+)\s*([А-Яа-яёЁ]+)\s*([А-Яа-яёЁ]+)$");
			if (inp != null && regex.IsMatch(inp))
				return true;
			else
				this.ErrorMessage = "Введите: Фамилия Имя Отчество";
				return false;
		}


	}
}
