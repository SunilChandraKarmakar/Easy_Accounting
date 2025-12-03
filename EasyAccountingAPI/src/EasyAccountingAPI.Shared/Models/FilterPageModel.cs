namespace EasyAccountingAPI.Shared.Models
{
    public class FilterPageModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterValue { get; set; }
    }
}