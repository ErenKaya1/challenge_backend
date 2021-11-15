namespace Challenge.Core.Request
{
    public class BaseFilter
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SearchTerm { get; set; }
    }
}