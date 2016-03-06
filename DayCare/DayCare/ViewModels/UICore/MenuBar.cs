using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.UICore
{
	public class MenuBar
	{
		//public static readonly MenuBar Empty;

		//static public MenuBar()
		//{
		//	Empty = new MenuBar();
		//}

		public delegate void MenuItemClicked();

		public virtual void OnCreateMenu() {}
	}

	public static class MenuBarExt
	{
		public static void Raise(this MenuBar.MenuItemClicked menu)
		{
			var handler = menu;
			if (handler != null)
			{
				handler();
			}
		}
	}
}
