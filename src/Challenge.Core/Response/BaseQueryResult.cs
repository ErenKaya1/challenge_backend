namespace Challenge.Core.Response
{
    public class BaseQueryResult<T>
    {
        public T Items { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}