using StackExchange.Redis;

namespace MonitoringAppAPI.Services
{
    public interface IRedisService
    {
        Task SaveTelemetryDataAsync(string key, string data, TimeSpan expiry);
        Task<string> GetTelemetryDataAsync(string key);
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

    }

}


