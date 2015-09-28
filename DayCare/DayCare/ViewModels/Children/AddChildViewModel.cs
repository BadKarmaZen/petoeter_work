using Caliburn.Micro;
using DayCare.Model.Database;
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
        private Child _child;

        public ChildDetailViewModel Detail { get; set; }


        public AddChildViewModel(Account account)
        {
            _account = account;
            _child = new Child { Id = Guid.NewGuid(), Account_Id = _account.Id };

            Detail = new ChildDetailViewModel(_child);
        }

        public void SaveAction()
        { }

        public void CancelAction()
        { }
    }
}
