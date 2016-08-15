using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DayCare.ViewModels.Dialogs
{
	public class SelectDriveViewModel : YesNoDialogViewModel
	{
		private List<DriveInfo> _drives;
		private DriveInfo _selectedDrive;

		public DriveInfo SelectedDrive
		{
			get { return _selectedDrive; }
			set { _selectedDrive = value; NotifyOfPropertyChange(() => SelectedDrive); }
		}

		public List<DriveInfo> Drives
		{
			get { return _drives; }
			set { _drives = value; NotifyOfPropertyChange(() => Drives); }
		}


		protected override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			try
			{
				var drives = from d in DriveInfo.GetDrives()
										 where d.DriveType == DriveType.Removable
										 select d;

				Drives = drives.ToList();
				SelectedDrive = Drives[0];
			}
			catch (Exception err)
			{
			}
		}
	}
}
