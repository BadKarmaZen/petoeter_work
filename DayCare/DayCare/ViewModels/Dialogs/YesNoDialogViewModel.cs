using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dialogs
{
    public class YesNoDialogViewModel : Screen
    {
        public string Message { get; set; }
        public System.Action Yes { get; set; }
        public System.Action No { get; set; }

        public void YesAction()
        {
            if (Yes != null)
            {
                Yes();                
            }

            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
               new Events.ShowDialog());
        }

        public void NoAction()
        {
            if (No != null)
            {
                No();
            }

            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
               new Events.ShowDialog());
        }
    
    }
}
