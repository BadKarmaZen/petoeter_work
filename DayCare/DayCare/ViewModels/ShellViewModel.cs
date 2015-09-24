using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels
{
    public class ShellViewModel : Screen,
        IHandle<Core.Events.SwitchTask>,
        IHandle<Core.Events.Close>
    {
        private Screen _task;
        private Screen _menu;

        public Screen Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                NotifyOfPropertyChange(() => Menu);
            }
        }

        public Screen Task
        {
            get
            {
                return _task;
            }

            set
            {
                _task = value;
                NotifyOfPropertyChange(() => Task);
            }
        }

        public ShellViewModel()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().Subscribe(this);

            Task = new Dashboard.DashBoardViewModel();
            //  Menu = new MainMenuBarViewModel();
        }

        public void Handle(Core.Events.SwitchTask message)
        {
            Task = message.Task;
        }

        public void Handle(Core.Events.Close message)
        {
            TryClose();
        }
    }
}
