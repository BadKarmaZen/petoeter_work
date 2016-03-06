using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.UICore
{
	public class BackMenu : MenuBar
	{
		#region Menu
		private MenuBar Menu;
		public string Id { get; set; }
		private Events.RegisterMenu MenuItem { get; set; }
		#endregion

		public event MenuItemClicked OnMenuClicked;

		public BackMenu(MenuBar Menu, string id,  MenuItemClicked clickedEvent)
		{
			OnMenuClicked = clickedEvent;
			this.Menu = Menu;
		}

		public override void OnCreateMenu()
		{
			MenuItem = new Events.RegisterMenu
			{
				Caption = "Terug",
				Id = Id,
				Add = true,
				Action = () =>
				{
					BackAction();
				}
			};


			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(MenuItem);

			this.Menu.OnCreateMenu();
		}

		protected virtual void BackAction()
		{
			OnMenuClicked.Raise();

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(new Events.RegisterMenu
			{
				Id = Id,
				Add = false
			});
		}
	}
}
