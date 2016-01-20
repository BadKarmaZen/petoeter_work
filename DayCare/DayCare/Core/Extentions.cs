using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.Core
{
	static class Date
	{
		public static DateTime NextMonday(DateTime? date = null)
		{
			if (!date.HasValue)
			{
				date = DateTime.Now;				
			}

			while (date.Value.DayOfWeek != DayOfWeek.Monday)
			{
				date = date.Value.AddDays(1);				
			}

			return date.Value;
		}
	}

	public static class StringExtention
	{
		public static BitmapImage LoadBitmapImage(this string fileName)
		{
			if (string.IsNullOrEmpty(fileName) || File.Exists(fileName) == false)
			{
				return null;				
			}

			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				MemoryStream ms = new MemoryStream();
				ms.SetLength(stream.Length);

				stream.Read(ms.GetBuffer(), 0, (int)stream.Length);
				ms.Flush();

				BitmapImage src = new BitmapImage();

				src.BeginInit();
				src.StreamSource = ms;
				src.EndInit();

				src.Freeze();

				return src;
			}
		}
	}
}
