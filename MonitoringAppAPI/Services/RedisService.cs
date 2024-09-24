using MonitoringAppAPI.Controllers;
using StackExchange.Redis;
using System.Text.Json;

namespace MonitoringAppAPI.Services
{
    public interface IRedisService
    {
        Task SaveTelemetryDataAsync(string key, string data, TimeSpan expiry);
        Task<string> GetTelemetryDataAsync(string key);
        Task<List<RedisData>> GetTelemetryDataAsync2(string baseId);
    }

    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task SaveTelemetryDataAsync(string key, string data, TimeSpan expiry)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, data, expiry);
        }

        public async Task<string> GetTelemetryDataAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<List<RedisData>> GetTelemetryDataAsync2(string baseId)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var pattern = $"{baseId}:*"; // Pattern to match all keys with the base ID
            var keys = await db.ExecuteAsync("KEYS", pattern); // Get all keys matching the pattern

            var entries = new List<RedisData>();

            // Retrieve each entry
            foreach (var key in (RedisKey[])keys)
            {
                var value = await db.StringGetAsync(key);
                if (value.HasValue)
                {
                    // convert value from string to object before adding
                    var valueObj = JsonSerializer.Deserialize<RedisData>(value);
                    entries.Add(valueObj);
                }
            }

            return entries;
        }

    }

}


