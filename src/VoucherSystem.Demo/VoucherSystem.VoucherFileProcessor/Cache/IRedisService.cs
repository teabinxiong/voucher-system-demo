using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSystem.VoucherFileProcessor.Cache
{
    public interface IRedisService
    {
        Task<string> GetRandomValueFromSetAsync(string key);
        Task InsertValuesIntoSetAsync(string key, IEnumerable<string> values);
        Task<IEnumerable<string>> GetAllValuesFromSetAsync(string key);
    }
}
