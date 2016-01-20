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
					string image = FindImageFile();
					return image.LoadBitmapImage();
				}
			}

			public bool ShowImage
			{
				get 
				{
					return !string.IsNullOrEmpty(FindImageFile());
				}
			}

			public string FindImageFile()
			{
				var model = ServiceProvider.Instance.GetService<Petoeter>();
				return Directory.EnumerateFiles(model.Settings.ImageFolder, string.Format("{0}*", Tag.Id.ToString())).FirstOrDefault();
			}
    }
}
