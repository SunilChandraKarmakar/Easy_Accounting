namespace EasyAccountingAPI.Model.Purchase
{
    [Table("PurchaseItems", Schema = "Purchase")]
    public class PurchaseItem : IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide purchase.")]
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "Please, provide product.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please, provide QTY.")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Please, provide unit price.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Please, provide sell price.")]
        public decimal SellPrice { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [Required(ErrorMessage = "Please, provide sub total price.")]
        public decimal SubTotal { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Purchase Purchase { get; set; }
        public Product Product { get; set; }
    }
}