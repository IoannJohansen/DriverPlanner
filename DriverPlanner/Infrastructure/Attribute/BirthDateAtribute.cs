using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverPlanner.Infrastructure.Attribute
{
	public class BirthDateAtribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			DateTime birthDate = (DateTime)value;
			if (birthDate>=DateTime.Today || birthDate.Year < DateTime.Today.Year - 120)
			{
				ErrorMessage = "Ваш возраст не соответствует требованиям";
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
