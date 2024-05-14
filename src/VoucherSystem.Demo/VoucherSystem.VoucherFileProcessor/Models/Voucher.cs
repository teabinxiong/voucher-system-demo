using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSystem.VoucherFileProcessor.Models
{
    public sealed class Voucher
    {
        public int CampaignId { get; set; }
        public int BatchId { get; set; }

        public string Code { get; set; }

        public Guid VoucherId { get; set; }


        public Voucher()
        {
            
        }

        public override string? ToString()
        {
            return $"CampaignId:{CampaignId};BatchId:{BatchId};Code:{Code};VoucherId:{VoucherId}";
        }
    }
}
