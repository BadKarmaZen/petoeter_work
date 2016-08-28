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
	public class ChildDetailViewModel : Screen, IHandle<Core.Events.SaveSnapshot>
	{
		#region Member
		private string _firstName;
		private string _lastName;
		private DateTime _birthDay;
		private string _fileId;
		private BitmapImage _imageData;
		#endregion

		public BitmapImage ImageData
		{
			get { return _imageData; }
			set { _imageData = value; NotifyOfPropertyChange(() => ImageData); }
		}
	
		public string FileID
		{
			get { return _fileId; }
			set 
			{ 
				_fileId = value;

				//	assign correct file id and need to upload
				ImageData = PetoeterImageManager.GetImage(FileID);

				NotifyOfPropertyChange(() => FileID);
				NotifyOfPropertyChange(() => ShowSelect);
				NotifyOfPropertyChange(() => ShowRemove);
			}
		}

		public bool ShowSelect
		{
			get
			{
				return string.IsNullOrEmpty(_fileId);
			}
		}

		public bool ShowRemove
		{
			get
			{
				return !string.IsNullOrEmpty(_fileId);
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

		public int ChildId { get; set; }

		public ChildDetailViewModel(Child child = null)
		{
			LogManager.GetLog(GetType()).Info("Create");
			if (child != null)
			{
				_firstName = child.FirstName;
				_lastName = child.LastName;
				_birthDay = child.BirthDay;
				FileID = child.FileId;
			}

			ServiceProvider.Instance.GetService<EventAggregator>().Subscribe(this);
		}

		internal void GetData(Child child)
		{
			child.FirstName = _firstName;
			child.LastName = _lastName;
			child.BirthDay = _birthDay;
			child.FileId = _fileId;
		}

		public void SelectImageAction()
		{
			LogManager.GetLog(GetType()).Info("Select image");
			var dlg = new Microsoft.Win32.OpenFileDialog();

			if (dlg.ShowDialog() == true)
			{
				ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.ShowSnapshot
					{
						FileName = dlg.FileName,
						FileId = string.Format("img/child/{0}", Guid.NewGuid())
					});
				
				//PetoeterImageManager.SaveFile(fileId, dlg.FileName);

				//FileID = fileId;
			}
		}

		public void RemoveImageAction()
		{
			LogManager.GetLog(GetType()).Info("Remove image");
			var name = FileID;
			PetoeterImageManager.RemoveFile(FileID);
			FileID = string.Empty;
		}

		public void Handle(Events.SaveSnapshot message)
		{
			FileID = message.FileId;
		}
	}
}
