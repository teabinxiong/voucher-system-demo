using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSystem.Demo.ApplicationServices
{
    public sealed class ServicesManager
    {

        CancellationTokenSource cts = new CancellationTokenSource();

        

        public ServicesManager()
        {
            
        }

        public void StartAllThread()
        {
            
        }

        public void StopAllThread()
        {
            Global.Logger.Information("StopAllThread");
         
            cts.Cancel();

            Global.Logger.Information("Wait for All theread to exit....");

            foreach (ManualResetEvent stopEvent in Global.ThreadCompleteEvents)
            {
                stopEvent.WaitOne();
            }

            Global.Logger.Information("Gracfully shutdown");
        }
    }
}
