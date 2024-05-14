using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSystem.Demo.ApplicationServices.WorkerServices.Abstraction
{
    public abstract class WorkerProcess
    {
        protected ManualResetEvent completeEvent = new ManualResetEvent(false);
        private bool stopThread = false;
        public abstract void StartThreadProc(object obj);

        public virtual void StopThread()
        {
            stopThread = true;
        }

        public bool IsThreadStopped()
        {
            return stopThread;
        }
    }
}
