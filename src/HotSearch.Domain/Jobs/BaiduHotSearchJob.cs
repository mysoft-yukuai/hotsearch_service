using HtmlAgilityPack;

namespace HotSearch.Domain.Jobs
{
    public class BaiduHotSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://top.baidu.com/board?tab=realtime";

        public override EnumHotSearchType Type => EnumHotSearchType.Baidu;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var body = await client.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(body);

            var titles = doc.DocumentNode.SelectNodes("//div[@class='c-single-text-ellipsis']");
            var linkUrls = doc.DocumentNode.SelectNodes("//a[@class='img-wrapper_29V76']");
            var heats = doc.DocumentNode.SelectNodes("//div[@class='hot-index_1Bl1a']");

            var list = new List<HotSearchModel>();

            for (var i = 0; i < heats.Count; i++)
            {
                var title = titles[i].InnerText.Trim();
                var linkUrl = linkUrls[i].GetAttributeValue("href", null);
                var sort = i + 1;
                var heat = heats[i].InnerText.Trim().ToLong();

                list.Add(new HotSearchModel
                {
                    Sort = sort,
                    Heat = heat,
                    LinkUrl = linkUrl,
                    Title = title,
                });
            }

            return list;
        }
    }
}
