using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSystem.VoucherFileProcessor.ApplicationServices
{
    public sealed class BackgroundService
    {
        private readonly ServicesManager _servicesManager;
        public BackgroundService(ServicesManager servicesManager)
        {
            _servicesManager = servicesManager;
        }

        public void Start()
        {
            Global.Logger.Information("Start Data Processing Service");

            _servicesManager.StartAllThread();

            Global.Logger.Information("All services started");
        }

        public void Stop()
        {
            Console.WriteLine("Quit Data Processing Service");

            Global.Logger.Information("Quit program!");
            _servicesManager.StopAllThread();
        }
    }
}
