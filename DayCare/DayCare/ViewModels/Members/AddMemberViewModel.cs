using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.ViewModels.Members
{
    public class AddMemberViewModel : Screen
    {
        private Account _account;

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; NotifyOfPropertyChange(() => FirstName); }
        }
        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; NotifyOfPropertyChange(() => LastName); }
        }

        public AddMemberViewModel(Account account)
        {
            // TODO: Complete member initialization
            this._account = account;
        }

        public void SaveAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
             new Events.ShowDialog());

            var member = new Member 
            {
                Id = Guid.NewGuid(), 
                Account_Id = _account.Id,
                FirstName = this.FirstName,
                LastName = this.LastName
            };

            ServiceProvider.Instance.GetService<Petoeter>().SaveMember(member);

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
