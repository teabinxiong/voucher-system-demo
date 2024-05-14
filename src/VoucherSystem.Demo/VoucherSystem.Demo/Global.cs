using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSystem.Demo
{
    public class Global
    {
        public static Serilog.ILogger Logger;

        public static List<ManualResetEvent> ThreadCompleteEvents = new List<ManualResetEvent>();
    }
}
