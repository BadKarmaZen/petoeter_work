using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.ViewModels.Children;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
    public class EditAccountViewModel : Screen
    {
        private Account _account;
        public AccountDetailViewModel Detail { get; set; }
        public ChildrenMainViewModel ChildrenDetail { get; set; }

        public EditAccountViewModel(Account account)
        {
            _account = account;

            Detail = new AccountDetailViewModel(_account);
            ChildrenDetail = new ChildrenMainViewModel(_account);
        }

        public void SaveAction()
        {
            var model = ServiceProvider.Instance.GetService<Petoeter>();
            model.UpdateRecord(_account);
            
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.SwitchTask
                {
                    Task = new AccountMainViewModel()
                });
        }

        public void CancelAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.SwitchTask
                {
                    Task = new AccountMainViewModel()
                });
        }
    }
}
