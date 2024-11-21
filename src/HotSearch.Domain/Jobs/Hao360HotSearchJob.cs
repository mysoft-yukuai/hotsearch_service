namespace HotSearch.Domain.Jobs
{
    public class Hao360HotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://ranks.hao.360.com/mbsug-api/hotnewsquery?type=news&realhot_limit=50";
        public override EnumHotSearchType Type => EnumHotSearchType.Hao360;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var json = await client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<JArray>(json);
            var rt = new List<HotSearchModel>();

            if (list is { Count: > 0 })
            {
                foreach (var item in list)
                {
                    var title = item.Value<string>("title");
                    var linkUrl = item.Value<string>("url");
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<long>("score");

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
