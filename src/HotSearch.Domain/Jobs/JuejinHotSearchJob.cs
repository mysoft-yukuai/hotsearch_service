namespace HotSearch.Domain.Jobs
{
    public class JuejinHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://api.juejin.cn/content_api/v1/content/article_rank?category_id=1&type=hot";
        public override EnumHotSearchType Type => EnumHotSearchType.Juejin;

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
                    var content = item.Value<JObject>("content");
                    var title = content?.Value<string>("title");
                    var linkUrl = "https://juejin.cn/post/" + content?.Value<string>("content_id");
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<JObject>("content_counter")?.Value<long>("hot_rank") ?? 0;

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
