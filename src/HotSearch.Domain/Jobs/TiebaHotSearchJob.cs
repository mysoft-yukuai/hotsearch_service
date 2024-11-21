namespace HotSearch.Domain.Jobs
{
    public class TiebaHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://tieba.baidu.com/hottopic/browse/topicList";
        public override EnumHotSearchType Type => EnumHotSearchType.Tieba;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var json = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<JObject>(json);

            var list = data?.Value<JObject>("data")?.Value<JObject>("bang_topic")?.Value<JArray>("topic_list");
            var rt = new List<HotSearchModel>();

            if (list is { Count: > 0 })
            {
                foreach (var item in list)
                {
                    var title = item.Value<string>("topic_name");
                    var linkUrl = item.Value<string>("topic_url");
                    var sort = list.IndexOf(item) + 1;
                    var heat = item.Value<long>("discuss_num");

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
