using System.Text.Json;

namespace StackExchange.Redis
{
    public static class IDatabaseExtend
    {
        public static async Task<List<T?>?> HashGetAllValueAsync<T>(this IDatabase db, RedisKey key)
        {
            var json = await db.HashGetAllAsync(key);
            if (json is not { Length: > 0 })
                return default;

            return json.Select(s => JsonSerializer.Deserialize<T>(s.Value!)).ToList();
        }

        public static async Task<T?> HashGetAsync<T>(this IDatabase db, RedisKey key, RedisValue field)
        {
            var json = await db.HashGetAsync(key, field);
            if (json.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(json!);
        }

        public static async Task<bool> HashSetAsync<T>(this IDatabase db, RedisKey key, RedisValue field, T val)
        {
            RedisValue redisVal = JsonSerializer.Serialize(val);

            return await db.HashSetAsync(key, field, redisVal);
        }
    }
}
