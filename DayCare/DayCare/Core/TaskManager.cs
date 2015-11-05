using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core
{
    public class BaseTask
    {
        public System.Action AbortAction { get; set; }
        //public Screen ViewModel { get; set; }
        public System.Action ReturnAction { get; set; }
    }

    public class TaskManager
    {
        public Queue<BaseTask> Tasks { get; set; }

        public BaseTask CurrentTask 
        { 
            get
            {
                if (Tasks.Count > 0)
                {
                    return Tasks.Peek();                    
                }

                return null;
            }
        }

        public TaskManager()
        {
            Tasks = new Queue<BaseTask>();
        }

        public void StartTask(BaseTask task)
        {
            Tasks.Enqueue(task);
        }

        public void EndTask()
        {
            if (Tasks.Count > 0)
            {
                Tasks.Dequeue();
            }
        }

        public void AbortTask()
        {
            if (Tasks.Count > 0)
            {
                BaseTask task = Tasks.Peek();
                if (task.AbortAction != null)
                {
                    task.AbortAction();
                }
                Tasks.Dequeue();
            }
        }
    }
}
