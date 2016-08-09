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
		private string _realcode;

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
			if(Code == _realcode)
				base.YesAction();
			else
				NoAction();
		}

		public void OneAction()
		{
			VerifyCode += "*";
			_realcode += "1";
		}

		public void TwoAction()
		{
			VerifyCode += "*";
			_realcode += "2";
		}

		public void ThreeAction()
		{
			VerifyCode += "*";
			_realcode += "3";
		}

		public void FourAction()
		{
			VerifyCode += "*";
			_realcode += "4";
		}

		public void FiveAction()
		{
			VerifyCode += "*";
			_realcode += "5";
		}

		public void SixAction()
		{
			VerifyCode += "*";
			_realcode += "6";
		}

		public void SevenAction()
		{
			VerifyCode += "*";
			_realcode += "7";
		}

		public void EightAction()
		{
			VerifyCode += "*";
			_realcode += "8";
		}

		public void NineAction()
		{
			VerifyCode += "*";
			_realcode += "9";
		}

		public void ZeroAction()
		{
			VerifyCode += "*";
			_realcode += "0";
		}

		public void DelAction()
		{
			if (VerifyCode.Length > 0)
			{
				VerifyCode = VerifyCode.Substring(0, VerifyCode.Length - 1);
				_realcode = _realcode.Substring(0, _realcode.Length - 1);
			}
		}
	}
}
