using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Children
{
    public class AddChildViewModel : Screen
    {
        private Account _account;

        public ChildDetailViewModel Detail { get; set; }


        public AddChildViewModel(Account account)
        {
            _account = account;
            Detail = new ChildDetailViewModel();
        }

        public void SaveAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
             new Events.ShowDialog());

            var child = new Child { Id = Guid.NewGuid(), Account_Id = _account.Id };
            Detail.GetData(child);

            ServiceProvider.Instance.GetService<Petoeter>().SaveChild(child);

            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.SwitchTask
                {
                    Task = new EditAccountViewModel(_account)
                });
        }

        public void CancelAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Events.ShowDialog());

            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.SwitchTask
                {
                    Task = new EditAccountViewModel(_account)
                });
        }
    }
}
