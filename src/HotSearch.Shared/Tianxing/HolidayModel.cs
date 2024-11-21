namespace HotSearch.Shared.Tianxing
{
    public class HolidayModel
    {
        public List<HolidayItemModel>? List { get; set; }
    }

    public class HolidayItemModel
    {
        /// <summary>  
        /// 当前阳历日期  
        /// </summary>  
        public DateOnly? Date { get; set; }

        /// <summary>  
        /// 日期类型，为0表示工作日、为1节假日、为2双休日、3为调休日（上班）  
        /// </summary>  
        public int? DayCode { get; set; }

        /// <summary>  
        /// 星期（数字）  
        /// </summary>  
        public int? Weekday { get; set; }

        /// <summary>  
        /// 星期（中文）  
        /// </summary>  
        public string? CnWeekday { get; set; }

        /// <summary>  
        /// 农历年  
        /// </summary>  
        public string? LunarYear { get; set; }

        /// <summary>  
        /// 农历月  
        /// </summary>  
        public string? LunarMonth { get; set; }

        /// <summary>  
        /// 农历日  
        /// </summary>  
        public string? LunarDay { get; set; }

        /// <summary>  
        /// 文字提示，工作日、节假日、节日、双休日、调休日  
        /// </summary>  
        public string? Info { get; set; }

        /// <summary>  
        /// 假期起点计数  
        /// </summary>  
        public int? Start { get; set; }

        /// <summary>  
        /// 假期当前计数
        /// </summary>  
        public int? Now { get; set; }

        /// <summary>  
        /// 假期终点计数  
        /// </summary>  
        public int? End { get; set; }

        /// <summary>  
        /// 节日日期  
        /// </summary>  
        public string? Holiday { get; set; }

        /// <summary>  
        /// 节假日名称（中文）  
        /// </summary>  
        public string? Name { get; set; }

        /// <summary>  
        /// 节日名称（英文）  
        /// </summary>  
        public string? EnName { get; set; }

        /// <summary>  
        /// 是否需要上班，0为工作日，1为休息日  
        /// </summary>  
        public int? IsNotWork { get; set; } 

        /// <summary>  
        /// 节假日数组
        /// </summary>  
        public List<DateOnly>? Vacation { get; set; }

        /// <summary>  
        /// 调休日数组
        /// </summary>  
        public List<DateOnly>? Remark { get; set; }

        /// <summary>  
        /// 薪资法定倍数/按年查询时为具体日期
        /// </summary>  
        public int? Wage { get; set; }

        /// <summary>  
        /// 放假提示
        /// </summary>  
        public string? Tip { get; set; }

        /// <summary>  
        /// 拼假建议
        /// </summary>  
        public string? Rest { get; set; }
    }
}
