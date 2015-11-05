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
    public class EditMemberViewModel : Screen
    {
        private Account _account;
        private Member _member;

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

        public EditMemberViewModel(Account account, Member member)
        {
            // TODO: Complete member initialization
            this._account = account;
            this._member = member;

            FirstName = _member.FirstName;
            LastName = _member.LastName;
        }

        public void SaveAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
             new Events.ShowDialog());

            _member.FirstName = FirstName;
            _member.LastName = LastName;

            ServiceProvider.Instance.GetService<Petoeter>().UpdateRecord(_member);

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
