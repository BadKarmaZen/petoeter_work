using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dialogs
{
	public class ChildPresenceUI
	{
		public string Name { get; set; }
		public Child Tag { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}

	public class AddPresenceDialogViewModel : YesNoDialogViewModel
	{
		public List<ChildPresenceUI> Children { get; set; }
		public ChildPresenceUI SelectedItem { get; set; }

		public AddPresenceDialogViewModel()
		{
			LogManager.GetLog(GetType()).Info("Create");

			var today = DateTimeProvider.Now().Date;

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var all = db.Children.FindAll().ToList();
				var exceptions = (from p in db.Presences.FindAll()
													select p.Child.Id).ToList();

				exceptions.ForEach(e => all.RemoveAll(c => c.Id == e));

				Children = (from c in all
										select new ChildPresenceUI
										{
											Name = c.GetFullName(),
											Tag = c
										}).ToList();
			}
		}

		public override void YesAction()
		{
			LogManager.GetLog(GetType()).Info("YesAction");

			if (SelectedItem != null)
			{				
				using (var db = new PetoeterDb(PetoeterDb.FileName))
				{
					var presence = new Presence
					{
						Child = SelectedItem.Tag,
						Date = DateTimeProvider.Now().Date,
						Updated = DateTimeProvider.Now(),
						TimeCode = 9
					};

					db.Presences.Insert(presence);
				};				
			}

			base.YesAction();
		}
	}
}
