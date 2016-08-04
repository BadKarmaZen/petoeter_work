using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dialogs
{
	class PasswordDialogViewModel : YesNoDialogViewModel
	{
		public string Code { get; set; }

		private string _verifyCode;

		public string VerifyCode
		{
			get { return _verifyCode; }
			set { _verifyCode = value; NotifyOfPropertyChange(() => VerifyCode); }
		}
		public PasswordDialogViewModel(string code)
		{
			this.Code = code;
		}

		public override void YesAction()
		{
			if(Code == VerifyCode)
				base.YesAction();
			else
				NoAction();
		}
	}
}
