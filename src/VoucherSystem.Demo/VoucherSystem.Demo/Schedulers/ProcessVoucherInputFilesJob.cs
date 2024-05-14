using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSystem.Demo.ApplicationServices.WorkerServices.Abstraction;
using VoucherSystem.Demo.Models;

namespace VoucherSystem.Demo.Schedulers
{
    [DisallowConcurrentExecution]
    public class ProcessVoucherInputFilesJob : IJob
    {
        private readonly IConfiguration _config;
        public ProcessVoucherInputFilesJob(IConfiguration config)
        {
            _config = config;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // Get Today Date
            DateTime today = DateTime.Now;
            var todayStr = today.ToString("dd-MM-yyyy");
            var targetFile = $"{todayStr}.csv";
            var voucherList = new List<Voucher>();
            var voucherDirectory = _config.GetSection("VoucherFolder").Value;
            var filePath = voucherDirectory + @"\" + targetFile;
            Global.Logger.Information($"File is {filePath}");
            // Check if there is any files exist
            var isFileExist = File.Exists(filePath);
            if (isFileExist)
            {
                Global.Logger.Information($"Start Processing {filePath}");

                var csvRows = System.IO.File.ReadAllLines(filePath, Encoding.Default).ToList();

                // read the file
            
                foreach (var row in csvRows.Skip(1))
                {
                    var voucher = new Voucher();
                    var columns = row.Split(',');
                    voucher.CampaignId = int.Parse(columns[0]);
                    voucher.BatchId = int.Parse(columns[1]);
                    voucher.Code = columns[2];
                    voucher.VoucherId = Guid.Parse(columns[3]);
                    voucherList.Add(voucher);
                }
            }else
            {
                Global.Logger.Information("File not found");
            }

            // insert into redis
            Global.Logger.Information("voucher generated");

            foreach(var voucher in voucherList)
            {
                Global.Logger.Information($"voucher={voucher.ToString()}");
            }

        }

    }
}
