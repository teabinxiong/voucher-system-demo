using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSystem.VoucherFileProcessor.ApplicationServices.WorkerServices.Abstraction;
using VoucherSystem.VoucherFileProcessor.Cache;

namespace VoucherSystem.VoucherFileProcessor.ApplicationServices.WorkerServices
{
    public sealed class VoucherConsumerSimulationWorker : WorkerProcess
    {
        private readonly IRedisService _redisService;
        public VoucherConsumerSimulationWorker(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public override void StartThreadProc(object obj)
        {
            Global.ThreadCompleteEvents.Add(completeEvent);
            var key = "1";
            // consume voucher from redis
            while(!IsThreadStopped())
            {
                // consume voucher from redis
                var takeOne =  _redisService.GetRandomValueFromSetAsync(key).Result;
                if (takeOne != null)
                {
                    Global.Logger.Information($"Thread-{Thread.CurrentThread.ManagedThreadId} - 1 voucher={takeOne.ToString()}");
                    Thread.Sleep(1000);
                }else
                {
                    Global.Logger.Information($"Thread-{Thread.CurrentThread.ManagedThreadId} - no voucher is available.");
                    Thread.Sleep(10000);
                }
            }

            Global.Logger.Information("VoucherConsumerSimulationWorker stopped.");
            completeEvent.Set();

        }
    }
}
