using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSystem.VoucherFileProcessor.Cache
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _redisDb;

        public RedisService(IDatabase redisDb)
        {
            _redisDb = redisDb;
        }

        public async Task<string> GetRandomValueFromSetAsync(string key)
        {
            var randomValue = await _redisDb.SetPopAsync(key);
            return randomValue.HasValue ? (string)randomValue : null;
        }

        public async Task InsertValuesIntoSetAsync(string key, IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                await _redisDb.SetAddAsync(key, value);
                await _redisDb.KeyExpireAsync(key, TimeSpan.FromMinutes(5));
            }
        }

        public async Task<IEnumerable<string>> GetAllValuesFromSetAsync(string key)
        {
            var setValues = await _redisDb.SetMembersAsync(key);
            return setValues.Select(s => (string)s);
        }
    }
}
