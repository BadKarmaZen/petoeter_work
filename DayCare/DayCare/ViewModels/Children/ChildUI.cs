using DayCare.Core;
using DayCare.Model.Lite;
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
				return PetoeterImageManager.GetImage(Tag.FileId);
			}
		}

		public bool ShowImage
		{
			get
			{
				return !string.IsNullOrEmpty(Tag.FileId);
			}
		}
	}
}
