using HtmlAgilityPack;
using System.Net.Http.Headers;

namespace HotSearch.Domain.Jobs
{
    public class HupuHtoSearchJob(IHttpClientFactory clientFactory, IConnectionMultiplexer redis) : BaseHotSearchJob(clientFactory, redis)
    {
        private const string url = "https://bbs.hupu.com/love-hot";
        public override EnumHotSearchType Type => EnumHotSearchType.Hupu;

        protected override async Task<List<HotSearchModel>> QueryHotSearchCore(HttpClient client)
        {
            var body = await client.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(body);

            var titles = doc.DocumentNode.SelectNodes("//a[@class='p-title']");
            var heats = doc.DocumentNode.SelectNodes("//div[@class='post-datum']");

            var list = new List<HotSearchModel>();

            for (var i = 0; i < heats.Count; i++)
            {
                var title = titles[i].InnerText.Trim();
                var linkUrl = "https://bbs.hupu.com/" + titles[i].GetAttributeValue("href", null);
                var sort = i + 1;
                var heat = heats[i].InnerText.Split('/')[1].Trim().ToLong();

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
