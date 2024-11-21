namespace HotSearch.Domain.Jobs
{
    public class WeiboHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://weibo.com/ajax/side/hotSearch";
        public override EnumHotSearchType Type => EnumHotSearchType.Weibo;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var json = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<JObject>(json);

            var list = data?.Value<JObject>("data")?.Value<JArray>("realtime");
            var rt = new List<HotSearchModel>();

            if (list is { Count: > 0 })
            {
                foreach (var item in list)
                {
                    var title = item.Value<string>("word");
                    var linkUrl = "https://s.weibo.com/weibo?q=" + title;
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<long>("num");

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
