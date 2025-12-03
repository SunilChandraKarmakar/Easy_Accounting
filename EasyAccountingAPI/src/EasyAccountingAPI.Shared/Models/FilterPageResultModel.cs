namespace EasyAccountingAPI.Shared.Models
{
    public class FilterPageResultModel<T> where T : class
    {
        public ICollection<T> Items { get; set; }
        public int TotalCount { get; set; }

        public FilterPageResultModel() { }

        public FilterPageResultModel(ICollection<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}