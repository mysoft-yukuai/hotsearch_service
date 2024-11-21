namespace HotSearch.Domain.Jobs
{
    public class TencentHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://r.inews.qq.com/gw/event/hot_ranking_list?page_size=51";
        public override EnumHotSearchType Type => EnumHotSearchType.Tencent;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var json = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<JObject>(json);

            var list = data?.Value<JArray>("idlist")?[0].Value<JArray>("newslist");
            var rt = new List<HotSearchModel>();

            if (list is { Count: > 0 })
            {
                foreach (var item in list)
                {
                    var sort = list.IndexOf(item) + 1;

                    if (sort == 1) continue;

                    var hot = item.Value<JObject>("hotEvent");
                    var title = hot?.Value<string>("title");
                    var linkUrl = item.Value<string>("url");
                    var heat = hot?.Value<long>("hotScore") ?? 0;

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
