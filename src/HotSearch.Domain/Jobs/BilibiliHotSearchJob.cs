namespace HotSearch.Domain.Jobs
{
    public class BilibiliHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://api.bilibili.com/x/web-interface/search/square?limit=50";
        public override EnumHotSearchType Type => EnumHotSearchType.Bilibili;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var json = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<JObject>(json);

            var list = data?.Value<JObject>("data")?.Value<JObject>("trending")?.Value<JArray>("list");
            var rt = new List<HotSearchModel>();

            if (list is { Count: > 0 })
            {
                foreach (var item in list)
                {
                    var title = item.Value<string>("show_name");
                    var linkUrl = "https://search.bilibili.com/all?keyword=" + item.Value<string>("keyword");
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<long>("heat_score");

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
    }
}
