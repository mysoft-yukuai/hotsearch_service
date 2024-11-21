namespace HotSearch.Domain.Jobs
{
    public interface IHotSearchJob
    {
        EnumHotSearchType Type { get; }
        Task GetHotSearch();
    }
}
