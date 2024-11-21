namespace HotSearch.Domain.Jobs
{
    public class DouyinHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://www.iesdouyin.com/web/api/v2/hotsearch/billboard/word/";
        public override EnumHotSearchType Type => EnumHotSearchType.Douyin;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var json = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<JObject>(json);

            var list = data?.Value<JArray>("word_list");
            var rt = new List<HotSearchModel>();

            if (list is { Count: > 0 })
            {
                foreach (var item in list)
                {
                    var title = item.Value<string>("word");
                    var linkUrl = "https://www.douyin.com/search/" + title + "?type=general";
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<long>("hot_value");

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
