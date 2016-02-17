using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Expenses
{
	class ExpenseDetailViewModel : Screen
	{
		public ExpenseDetailViewModel(Child child)
		{

		}

		public void CloseAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
			 new Events.ShowDialog());
		}
	}
}
