using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.ViewModels.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Database
{
	public class DatabaseViewModel : Screen
	{
		public void ExportAction()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();
			if (model != null)
			{
				var dlg = new System.Windows.Forms.FolderBrowserDialog();
				
				var result = dlg.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					model.Export(dlg.SelectedPath);

					ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
						new Events.ShowDialog
						{
							Dialog = new OkDialogViewModel
							{
								Message = "Export is uitgevoerd!"
							}
						});
				}
			}
		}

		public void ImportAction()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();
			if (model != null)
			{
				var dlg = new System.Windows.Forms.FolderBrowserDialog();

				var result = dlg.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					model.Import(dlg.SelectedPath);

					ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
						new Events.ShowDialog
						{
							Dialog = new OkDialogViewModel
							{
								Message = "Import is uitgevoerd!"
							}
						});
				}
			}
		}
	}
}
