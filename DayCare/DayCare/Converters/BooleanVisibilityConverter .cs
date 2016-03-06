using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DayCare.Converters
{
	public class BooleanVisibilityConverter : IValueConverter
	{
		#region IValueConverter Implementation
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? boolValue = (bool?)value;

			switch (boolValue)
			{
				case null:
					return Visibility.Collapsed;

				case true:
					return Visibility.Visible;

				case false:
					return Visibility.Collapsed;

				default:
					return value;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
		#endregion
	}

	public class InverseBooleanVisibilityConverter : IValueConverter
	{
		#region IValueConverter Implementation
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? boolValue = (bool?)value;

			switch (!boolValue)
			{
				case null:
					return Visibility.Collapsed;

				case true:
					return Visibility.Visible;

				case false:
					return Visibility.Collapsed;

				default:
					return value;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
		#endregion
	}
}
