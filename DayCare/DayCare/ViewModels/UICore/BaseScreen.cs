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
		public MenuBar Menu { get; set; }

		public BaseScreen()
		{
			Menu = new MenuBar();
		}

		public virtual void CloseThisScreen()
		{
		}

		protected override void OnViewAttached(object view, object context)
		{
			if (Menu != null)
			{

				Menu.OnCreateMenu();				
			}
			
			base.OnViewAttached(view, context);
		}
	}
}
