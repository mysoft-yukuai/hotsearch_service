using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace HotSearch.Host.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class BaseController(IConnectionMultiplexer redis) : ControllerBase
    {
        protected readonly IConnectionMultiplexer _redis = redis;
    }
}
