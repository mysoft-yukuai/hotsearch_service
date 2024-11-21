using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HotSearch.Domain.Jobs
{
    public abstract class BaseHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : IHotSearchJob
    {
        protected string NAME => Type.ToString();
        public abstract EnumHotSearchType Type { get; }
        protected readonly IHttpClientFactory _clientFactory = clientFactory;
        protected readonly IConnectionMultiplexer _redis = redis;

        public virtual async Task GetHotSearch()
        {
            using var client = _clientFactory.CreateClient(NAME);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");
            try
            {
                var list = await QueryHotSearchCore(client);

                if (list is { Count: > 0 })
                {
                    await Push(list);
                }
            }
            catch (Exception)
            {
            }
        }

        protected abstract Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client);

        protected virtual async Task Push(List<HotSearchModel> list)
        {
            var db = _redis.GetDatabase();
            RedisValue oldlist = await db.HashGetAsync(RedisKeyNames.HotSearch, NAME);
            RedisValue newlist = JsonSerializer.Serialize(list);
            bool isdiff = oldlist.IsNullOrEmpty || oldlist != newlist;

            if (await db.HashExistsAsync(RedisKeyNames.HotSearch, NAME))
                await db.HashDeleteAsync(RedisKeyNames.HotSearch, NAME);

            await db.HashSetAsync(RedisKeyNames.HotSearch, NAME, newlist);

            if (isdiff)
                await db.HashSetAsync(RedisKeyNames.HotSearchLast, NAME, DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }
    }
}
