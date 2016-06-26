
using DayCare.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.Core
{
	[Obsolete("", true)]
	class ImageManager
	{
		private Dictionary<string, BitmapImage> _cache;

		public string ImageFolder { get; set; }

		public ImageManager ()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			if (model != null)
			{
				ImageFolder = model.Settings.ImageFolder;
			}

			_cache = new Dictionary<string, BitmapImage>();
		}

		public string FindImage(string id)
		{
			if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(ImageFolder))
				return string.Empty;

			return Directory.EnumerateFiles(ImageFolder, string.Format("{0}*", id)).FirstOrDefault();
		}

		public BitmapImage CreateBitmap(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return null;
			}

			BitmapImage image;

			if (_cache.TryGetValue(fileName, out  image))
			{
				return image;						
			}
			
			if (File.Exists(fileName) == false)
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

				_cache.Add(fileName, src);

				return src;
			}
		}
	}
	
}
