using System.Text.RegularExpressions;

namespace HotSearch.Domain.Jobs
{
    public class ZhihuHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://www.zhihu.com/api/v3/feed/topstory/hot-lists/total";
        public override EnumHotSearchType Type => EnumHotSearchType.Zhihu;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var json = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<JObject>(json);

            var list = data?.Value<JArray>("data");
            var rt = new List<HotSearchModel>();

            if (list is { Count: > 0 })
            {
                foreach (var item in list)
                {
                    var target = item.Value<JObject>("target");
                    var title = target?.Value<string>("title");
                    var linkUrl = "https://www.zhihu.com/question/" + target?.Value<string>("id");
                    var sort = list.IndexOf(item) + 1;
                    var heat = ConvertNumber(item.Value<string>("detail_text")??"");

                    rt.Add(new HotSearchModel
                    {
                        Sort = sort,
                        Heat = heat,
                        LinkUrl = linkUrl,
                        Title = title,
                    });
                }
            }

            return rt;
        }

        private static long ConvertNumber(string input)
        {
            string pattern = @"(\d+(\.\d+)?)\s万热度";

            return Regex.Replace(input, pattern, match =>
            {
                // 提取匹配到的数字部分  
                string numberStr = match.Groups[1].Value;

                return (numberStr.ToLong() * 10_000).ToString();
            }).ToLong();
        }
    }
}
