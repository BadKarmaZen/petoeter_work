using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.Model.Tasks;
using DayCare.ViewModels.Children;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
    public class AddAccountViewModel : Screen
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(() => Name); }
        }
        public void SaveAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
              new Events.ShowDialog());
            
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = _name
            };

            ServiceProvider.Instance.GetService<Petoeter>().SaveAccount(account);
            
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.SwitchTask
                {
                    Task = new EditAccountViewModel(account)
                });
        }

        public void CancelAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
              new Events.ShowDialog());

            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.SwitchTask
                {
                    Task = new AccountMainViewModel()
                });
        }
       

    }
    //public class AddAccountViewModel : ReactivatableScreen
    //{
    //    private Account _account;
    //    public AccountDetailViewModel Detail { get; set; }
    //    public ChildrenMainViewModel ChildrenDetail { get; set; } 

    //    public AddAccountViewModel(Account account = null)
    //    {
    //        if (account == null)
    //        {
    //            _account = new Account { Id = Guid.NewGuid() };                
    //        }
    //        else
    //        {
    //            _account = account;
    //        }

    //        Detail = new AccountDetailViewModel(_account);
    //        ChildrenDetail = new ChildrenMainViewModel(_account); 
            
    //        var tasks = ServiceProvider.Instance.GetService<TaskManager>();
    //        tasks.StartTask(new AddAccountTask
    //        {
    //            ReturnAction = () =>
    //                ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
    //                    new Core.Events.SwitchTask
    //                    {
    //                        Task = new AddAccountViewModel(_account)
    //                    })
    //        });
    //    }

    //    public override void Reactivate()
    //    {
    //        ChildrenDetail.Reactivate();
    //    }

    //    public void SaveAction()
    //    {
    //        //  get data
    //        _account.Name = Detail.Name;
    //        _account.Children = (from c in ChildrenDetail.Children
    //                             select c.Tag).ToList();
 
    //        ServiceProvider.Instance.GetService<Petoeter>().SaveRecord(_account);
    //        ServiceProvider.Instance.GetService<TaskManager>().EndTask();
    //        ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
    //            new Core.Events.SwitchTask
    //            {
    //                Task = new AccountMainViewModel()
    //            });
    //    }

    //    public void CancelAction()
    //    {
    //        ServiceProvider.Instance.GetService<TaskManager>().EndTask();
    //        ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
    //            new Core.Events.SwitchTask
    //            {
    //                Task = new AccountMainViewModel()
    //            });
    //    }
    //}    
}
