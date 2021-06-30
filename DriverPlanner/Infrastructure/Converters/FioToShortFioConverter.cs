using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DriverPlanner.Infrastructure.Converters
{
	public class FioToShortFioConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string longFio = System.Convert.ToString(value);
			if (value == null) return "";
			// fix the array of less size then need
			var fioSplitted = longFio.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries );
			return fioSplitted.Length != 0 ? $"{fioSplitted[0]} {fioSplitted[1].Substring(0, 1)} {fioSplitted[2].Substring(0, 1)}" : null;	
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
