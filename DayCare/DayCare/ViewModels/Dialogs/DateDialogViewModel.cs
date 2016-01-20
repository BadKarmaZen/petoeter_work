using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dialogs
{
	public class DateDialogViewModel : YesNoDialogViewModel
	{
		private DateTime _date;

		public DateTime Date
		{
			get { return _date; }
			set { _date = value; NotifyOfPropertyChange(() => Date); }
		}
	}
}
