using StackExchange.Redis;
using System.Text.Json;

namespace VkEgoGraphApi;

public class CacheService
{
    private static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost:6380", c => { });
    private static IDatabase _db = _redis.GetDatabase();

    public T Get<T>(string key) where T : class
    {
        var payload = _db.StringGet(key).ToString();

        if (string.IsNullOrEmpty(payload)) return null;

        try
        {
            return JsonSerializer.Deserialize<T>(payload);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public void Set<T>(string key, T payload) where T : class
    {
        string json = JsonSerializer.Serialize<T>(payload);

        _db.StringSet(key, json);
    }
}