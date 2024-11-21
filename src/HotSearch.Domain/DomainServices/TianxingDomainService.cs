using HotSearch.Shared.Options;
using HotSearch.Shared.Tianxing;
using Microsoft.Extensions.Options;

namespace HotSearch.Domain.DomainServices
{
    public class TianxingDomainService(IHttpClientFactory clientFactory, IConnectionMultiplexer redis,IOptions<TianXingOption> options)
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IConnectionMultiplexer _redis = redis;
        private readonly TianXingOption _option = options.Value;
        private string? ApiKey => _option.ApiKey;
        private string? BaseUrl => _option.BaseUrl;
        /// <summary>
        /// 获取节假日
        /// </summary>
        /// <param name="date">指定年月</param>
        /// <returns></returns>
        public async Task<List<HolidayItemModel>?> GetHoliday(DateTime date)
        {
            var month = date.ToString("yyyy-MM");
            string url = $"{BaseUrl}/jiejiari/index?key={ApiKey}&date={month}&type=2";
            using var client = _clientFactory.CreateClient("Tianxing");
            var json = await client.GetStringAsync(url);

            var data = JsonConvert.DeserializeObject<ResultModel<HolidayModel>>(json);
            if (data is { Code: 200 } && data?.Result?.List is { Count: > 0 })
            {
                var db = _redis.GetDatabase();
                await db.HashSetAsync(RedisKeyNames.Holiday, month, data.Result.List);
                return data.Result.List;
            }
            return null;
        }

        /// <summary>
        /// 获取古诗
        /// </summary>
        /// <returns></returns>
        public async Task InitPoetry()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd");
            var db = _redis.GetDatabase();
            if (!await db.HashExistsAsync(RedisKeyNames.Poetry, now))
            {
                var list = new List<PoetryModel>();
                string url = $"{BaseUrl}/qingshi/index?key={ApiKey}";
                using var client = _clientFactory.CreateClient("Tianxing");
                await Parallel.ForAsync(1, 100, async (i, token) =>
                {
                    var json = await client.GetStringAsync(url, token);

                    var data = JsonConvert.DeserializeObject<ResultModel<PoetryModel>>(json);

                    if (data is { Code: 200 } && data?.Result is not null)
                        list.Add(data.Result);
                });

                await db.HashSetAsync(RedisKeyNames.Poetry, now, list);
            }            
        }

        /// <summary>
        /// 打工人语录
        /// </summary>
        /// <returns></returns>
        public async Task InitSentence()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd");
            var db = _redis.GetDatabase();
            if (!await db.HashExistsAsync(RedisKeyNames.Sentence, now))
            {
                var list = new List<string>();
                string url = $"{BaseUrl}/dgryl/index?key={ApiKey}";
                using var client = _clientFactory.CreateClient("Tianxing");
                await Parallel.ForAsync(1, 100, async (i, token) =>
                {
                    var json = await client.GetStringAsync(url, token);

                    var data = JsonConvert.DeserializeObject<ResultModel<ContentModel>>(json);

                    var content = data?.Result?.Content;

                    if (data is { Code: 200 } && !string.IsNullOrEmpty(content))
                    {
                        list.Add(content);
                    }
                });

                await db.HashSetAsync(RedisKeyNames.Sentence, now, list);
            }
        }
    }
}
