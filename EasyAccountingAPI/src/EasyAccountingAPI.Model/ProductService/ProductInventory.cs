namespace EasyAccountingAPI.Model.ProductService
{
    [Table("ProductInventories", Schema = "ProductService")]
    public class ProductInventory : IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product id must be greater than 0.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Opening Stock is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Opening stock must be greater than 0.")]
        public decimal OpeningStock { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public bool HaveStockAlert { get; set; }
        public decimal? StockAlertQty { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Product Product { get; set; }
    }
}