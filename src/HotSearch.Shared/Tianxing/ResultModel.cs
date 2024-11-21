namespace HotSearch.Shared.Tianxing
{
    public class ResultModel<T> : BaseModel where T : class, new()
    {
        public T? Result { get; set; }
    }
}
