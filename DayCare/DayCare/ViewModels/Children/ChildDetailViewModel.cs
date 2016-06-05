using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.ViewModels.Children
{
	public class ChildDetailViewModel : Screen
	{
		#region Member
		private string _firstName;
		private string _lastName;
		private DateTime _birthDay;
		private string _image;
		private BitmapImage _imageData;
		#endregion

		public BitmapImage ImageData
		{
			get { return _imageData; }
			set { _imageData = value; NotifyOfPropertyChange(() => ImageData); }
		}
	
		public string Image
		{
			get { return _image; }
			set 
			{ 
				_image = value;

				var img = ServiceProvider.Instance.GetService<ImageManager>();
				ImageData = img.CreateBitmap(_image);

				NotifyOfPropertyChange(() => Image);
				NotifyOfPropertyChange(() => ShowSelect);
				NotifyOfPropertyChange(() => ShowRemove);
			}
		}

		public bool ShowSelect
		{
			get
			{
				return string.IsNullOrEmpty(_image);
			}
		}

		public bool ShowRemove
		{
			get
			{
				return !string.IsNullOrEmpty(_image);
			}
		}

		public DateTime BirthDay
		{
			get { return _birthDay; }
			set { _birthDay = value; NotifyOfPropertyChange(() => BirthDay); }
		}

		#region Properties
		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; NotifyOfPropertyChange(() => FirstName); }
		}

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; NotifyOfPropertyChange(() => FirstName); }
		}
		#endregion

		public Guid ChildId { get; set; }

		public ChildDetailViewModel(Child child = null)
		{
			if (child != null)
			{
				_firstName = child.FirstName;
				_lastName = child.LastName;
				_birthDay = child.BirthDay;
				//ChildId = child.Id;

				var img = ServiceProvider.Instance.GetService<ImageManager>();

				Image = img.FindImage(ChildId.ToString());
			}
		}

		internal void GetData(Child child)
		{
			child.FirstName = _firstName;
			child.LastName = _lastName;
			child.BirthDay = _birthDay;
			//child.Id = ChildId;
		}

		public void SelectImageAction()
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();

			if (dlg.ShowDialog() == true)
			{
				//var model = ServiceProvider.Instance.GetService<Petoeter>();

				//string file = dlg.FileName;
				//string extention = Path.GetExtension(file);
				//string destination = Path.Combine(model.Settings.ImageFolder, string.Format( "{0}.{1}", ChildId.ToString(), extention));

				//File.Copy(file, destination, true);
				
				//Image = file;				
			}
		}

		public void RemoveImageAction()
		{
			var name = Image;
			Image = string.Empty;

			if (File.Exists(name))
			{
				File.Delete(name);
			}
		}
	}
}
