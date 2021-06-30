using DriverPlanner.DPService;
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
	class IndexToCarImage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int indexOFimage = (int)value;
			if (indexOFimage == 0)
			{

				const string defPath = @"D:\Desktop\Labs\DriverPlanner\DriverPlanner\Resources\Images\default_car.jpg";
				var defImg = ImageConverter.ImageConverter.ImageToBytes(defPath);
				return defImg;
			}
			else
			{
				using (DriverPlannerServiceClient dps = new DriverPlannerServiceClient())
				{
					return dps.GetImage(indexOFimage);
				}

			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
