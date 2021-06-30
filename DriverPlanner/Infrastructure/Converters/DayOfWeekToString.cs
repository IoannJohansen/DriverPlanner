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
	public class DayOfWeekToString : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (((DateTime)value).DayOfWeek)
			{
				case DayOfWeek.Monday: return "ПН";
				case DayOfWeek.Sunday: return "ВС";
				case DayOfWeek.Tuesday: return "ВТ";
				case DayOfWeek.Wednesday: return "СР";
				case DayOfWeek.Thursday: return "ЧТ";
				case DayOfWeek.Friday: return "ПТ";
				case DayOfWeek.Saturday: return "СБ";
				default: return "DEF";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> DependencyProperty.UnsetValue;
	}
}
