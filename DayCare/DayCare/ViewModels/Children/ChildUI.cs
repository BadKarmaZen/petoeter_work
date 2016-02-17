using DayCare.Core;
using DayCare.Model.Database;
using DayCare.Model.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.ViewModels.Children
{
	public class ChildUI : TaggedItemUI<Child>
	{
		public BitmapImage ImageData
		{
			get
			{
				var img = ServiceProvider.Instance.GetService<ImageManager>();

				return img.CreateBitmap(img.FindImage(Tag.Id.ToString()));
			}
		}

		public bool ShowImage
		{
			get
			{
				var img = ServiceProvider.Instance.GetService<ImageManager>();
				return !string.IsNullOrEmpty(img.FindImage(Tag.Id.ToString()));
			}
		}
	}
}
