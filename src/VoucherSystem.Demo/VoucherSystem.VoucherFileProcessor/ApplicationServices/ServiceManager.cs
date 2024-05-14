using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSystem.VoucherFileProcessor.ApplicationServices.WorkerServices;
using VoucherSystem.VoucherFileProcessor.ApplicationServices.WorkerServices.Abstraction;

namespace VoucherSystem.VoucherFileProcessor.ApplicationServices
{
    public sealed class ServicesManager
    {

        CancellationTokenSource cts = new CancellationTokenSource();
        List<WorkerProcess> workerProcesses= new List<WorkerProcess>();
         private readonly IServiceProvider _services;
        public ServicesManager(IServiceProvider services)
        {
            this._services = services;
        }

        public void StartAllThread()
        {
            using (var scope = _services.CreateScope())
            {
                
                for (var i = 0; i < Global.MaxThread; i++)
                {
                    var simulationWorkers = scope.ServiceProvider.GetRequiredService<VoucherConsumerSimulationWorker>();

                    ThreadPool.QueueUserWorkItem(simulationWorkers.StartThreadProc, cts);
                }
            }
        }

        public void StopAllThread()
        {
            Global.Logger.Information("StopAllThread");
         
            cts.Cancel();

            foreach(var process in workerProcesses)
            {
                process.StopThread();
            }

            Global.Logger.Information("Wait for All theread to exit....");

            foreach (ManualResetEvent stopEvent in Global.ThreadCompleteEvents)
            {
                stopEvent.WaitOne();
            }

            Global.Logger.Information("Gracfully shutdown");
        }
    }
}
