using HotSearch.Shared.Enums;

namespace HotSearch.Shared.Model
{
    public class HotSearchModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string? LinkUrl { get; set; }
        /// <summary>
        /// 热搜排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 热搜指数
        /// </summary>
        public long Heat { get; set; }
    }

    public class HotSearchVo : HotSearchModel
    {
        public EnumHotSearchType Type { get; set; }
    }
}
