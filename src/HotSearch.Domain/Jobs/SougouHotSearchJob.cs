namespace HotSearch.Domain.Jobs
{
    public class SougouHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://go.ie.sogou.com/hot_ranks";
        public override EnumHotSearchType Type => EnumHotSearchType.Sougou;

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
                    var attribute = item.Value<JObject>("attributes");
                    var title = attribute?.Value<string>("title");
                    var linkUrl = "https://www.sogou.com/web?ie=utf8&query=" + title;
                    var sort = list.IndexOf(item) + 1;
                    var heat = attribute?.Value<long>("num") ?? 0;

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
