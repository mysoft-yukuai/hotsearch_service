namespace HotSearch.Domain
{
    public static class RedisKeyNames
    {
        public static RedisKey InitData => "InitData";
        public static RedisKey Poetry => "Poetry";
        public static RedisKey Sentence => "Sentence";
        public static RedisKey Holiday => "Holiday";
        public static RedisKey HotSearch => "HotSearch";
        public static RedisKey HotSearchLast => "HotSearchLast";
        public static RedisKey Visitor => "Visitor";
        public static RedisKey VisitorToday => $"VisitorToday:{DateTime.Today:yyyyMMdd}";
        public static RedisKey VisitorUser => "VisitorUser";
        public static RedisKey VisitorUserToday => $"VisitorUserToday:{DateTime.Today:yyyyMMdd}";
    }
}
