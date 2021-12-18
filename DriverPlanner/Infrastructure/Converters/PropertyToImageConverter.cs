using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DriverPlanner.Infrastructure.Converters
{
	public class PropertyToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			const string defPath = @"D:\\Desktop\\4d671cfe78cbb63a617875e1f6023157.jpeg";
			var res = DriverPlanner.ImageConverter.ImageConverter.ImageToBytes(defPath);
			byte[] img = value as byte[];
			if (img == null) return res;
			else return img;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
