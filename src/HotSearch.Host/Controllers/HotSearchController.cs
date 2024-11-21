using HotSearch.Domain;
using HotSearch.Shared.Enums;
using HotSearch.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace HotSearch.Host.Controllers
{
    public class HotSearchController(IConnectionMultiplexer redis) : BaseController(redis)
    {
        [HttpGet]
        public async Task<HotSearchRes?> GetHotSearch(EnumHotSearchType searchType)
        {
            if (searchType == EnumHotSearchType.UnKnown)
                return null;

            var db = _redis.GetDatabase();

            var val = await db.HashGetAsync(RedisKeyNames.HotSearch, searchType.ToString());
            if (val.IsNullOrEmpty)
                return null;

            var last = await db.HashGetAsync(RedisKeyNames.HotSearchLast, searchType.ToString());
            return new HotSearchRes
            {
                LastUpdate = last.IsNullOrEmpty ? null : last.ToLong(),
                Items = JsonSerializer.Deserialize<List<HotSearchModel>>(val!)
            };
        }

        [HttpGet]
        public async Task<List<HotSearchVo>?> QueryHotSearchByKeyword(string keyword)
        {
            var list = new List<HotSearchVo>();
            var types = Enum.GetValues<EnumHotSearchType>();
            var db = _redis.GetDatabase();
            foreach (var type in types)
            {
                var data = await db.HashGetAsync<List<HotSearchModel>>(RedisKeyNames.HotSearch, type.ToString());
                if (data is { Count: > 0 })
                    list.AddRange(data.Select(s => new HotSearchVo
                    {
                        Sort = s.Sort,
                        Heat = s.Heat,
                        LinkUrl = s.LinkUrl,
                        Title = s.Title,
                        Type = type,
                    }));
            }

            return list.Where(s => s.Title!.Contains(keyword)).Take(10).ToList();
        }
    }
}
