using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dashboard
{
    public class DashBoardViewModel : Screen
    {
        public void CloseAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.Close());
        }

        public void AdministrationAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                  new Core.Events.SwitchTask
                  {
                      //Task = new MainAccountViewModel()
                  });
        }

        public void StartPrecenseAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.SwitchTask
                {
                    //Task = new LoginViewModel()
                });
        }
    }
}
