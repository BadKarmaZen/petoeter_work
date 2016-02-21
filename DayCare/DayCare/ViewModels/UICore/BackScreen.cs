using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.UICore
{
	public class BackScreen<T> : BaseScreen
		where T: BaseScreen
	{
		private Events.RegisterMenu AddBackMenu { get; set; }
		private Events.RegisterMenu RemoveBackMenu { get; set; }

		public BackScreen()
		{
			AddBackMenu = new Events.RegisterMenu
			{
				Caption = "Terug",
				Id = Guid.NewGuid().ToString(),
				Add = true,
				Action = () =>
				{
					BackAction();
				}
			};

			RemoveBackMenu = new Events.RegisterMenu
			{
				Id = AddBackMenu.Id,
				Add = false
			};


			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
		}

		public override void CloseThisScreen()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(RemoveBackMenu);

			base.CloseThisScreen();
		}

		public virtual void BackAction()
		{

		}
	}
}
