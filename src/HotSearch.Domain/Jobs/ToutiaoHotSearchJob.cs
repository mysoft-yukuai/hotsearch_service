namespace HotSearch.Domain.Jobs
{
    public class ToutiaoHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://www.toutiao.com/hot-event/hot-board/?origin=toutiao_pc";
        public override EnumHotSearchType Type => EnumHotSearchType.Toutiao;

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
                    var title = item.Value<string>("Title");
                    var linkUrl = item.Value<string>("Url");
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<long>("HotValue");

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
