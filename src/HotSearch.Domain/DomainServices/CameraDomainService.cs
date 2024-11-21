using HtmlAgilityPack;

namespace HotSearch.Domain.DomainServices
{
    public class CameraDomainService(IHttpClientFactory clientFactory, IConnectionMultiplexer redis)
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IConnectionMultiplexer _redis = redis;
        public const string NAME = "Camera";
        private const string url = "https://www.skylinewebcams.com/zh/webcam.html";
        public async Task InitCamera()
        {
            var db = _redis.GetDatabase();
            var cameras = await db.HashGetAsync<List<CameraModel>>(RedisKeyNames.InitData, NAME);

            if (cameras is { Count: >= 5 })
                return;

            using var client = _clientFactory.CreateClient(NAME);
            var body = await client.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(body);
            var items = doc.DocumentNode.SelectNodes("//div[@class='row list']/a");
            cameras = [];
            foreach (var item in items)
            {
                var title = item.SelectSingleNode(".//p[@class='tcam']").InnerText.Trim();
                var linkurl = "https://www.skylinewebcams.com/" + item.GetAttributeValue("href", "");
                cameras.Add(new CameraModel
                {
                    Name = title,
                    Url = linkurl,
                });
            }

            await db.HashSetAsync(RedisKeyNames.InitData, NAME, cameras);
        }
    }
}
