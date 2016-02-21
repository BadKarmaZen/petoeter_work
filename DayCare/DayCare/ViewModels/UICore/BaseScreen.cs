using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.ViewModels.UICore
{
	public class BaseScreen : Screen, ICloseScreen
	{
		public virtual void CloseThisScreen()
		{
		}
	}
}
