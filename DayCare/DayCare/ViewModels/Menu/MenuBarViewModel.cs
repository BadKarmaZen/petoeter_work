using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Menu
{
	public class MenuItem
	{
		public string Caption { get; set; }
		public string Id { get; set; }
		public System.Action Action { get; set; }
		public int Level { get; set; }
	}

	public class MenuBarViewModel : Screen, IHandle<Events.RegisterMenu>
	{
		#region Members

		private List<MenuItem> _menuItems = new List<MenuItem>();
		public int CurrentLevel { get; set; }

		#endregion

		#region Properties
		public List<MenuItem> MenuItems
		{
			get { return new List<MenuItem>(_menuItems); }
			set { _menuItems = value; }
		}
		
		#endregion

		public MenuBarViewModel()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().Subscribe(this);
		}

		public void ExecuteAction(MenuItem menu)
		{
			LogManager.GetLog(GetType()).Info("Execute menu");
			menu.Action();
		}

		public void Handle(Events.RegisterMenu message)
		{
			if (message.Add)
			{
				LogManager.GetLog(GetType()).Info("Handle.Add");

				var menu = _menuItems.Find(m => m.Id == message.Id);

				if (menu != null)
				{
					_menuItems.RemoveAll(m => m.Id == message.Id);
				}

				_menuItems.Add(new MenuItem
				{
					Caption = message.Caption,
					Id = message.Id,
					Action = message.Action,
					Level = CurrentLevel++
				});

				NotifyOfPropertyChange(() => MenuItems);
			}
			else
			{
				LogManager.GetLog(GetType()).Info("Handle.Add=false");

				var menu = _menuItems.FirstOrDefault(m => m.Id == message.Id);

				if (menu != null)
				{
					_menuItems.RemoveAll(m => m.Level >= menu.Level);
					NotifyOfPropertyChange(() => MenuItems);					
				}
			}
		}
	}
}
