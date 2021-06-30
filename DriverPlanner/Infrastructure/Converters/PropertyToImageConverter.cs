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
			const string defPath = @"D:\Desktop\Labs\DriverPlanner\DriverPlanner\Resources\Images\defbg.png";
			var res = ImageConverter.ImageConverter.ImageToBytes(defPath);
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
