namespace HotSearch.Domain.Jobs
{
    public class CsdnHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://blog.csdn.net/phoenix/web/blog/hotRank?&pageSize=50";
        public override EnumHotSearchType Type => EnumHotSearchType.Csdn;

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
                    var title = item.Value<string>("articleTitle");
                    var linkUrl = item.Value<string>("articleDetailUrl");
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<long>("hotRankScore");

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
