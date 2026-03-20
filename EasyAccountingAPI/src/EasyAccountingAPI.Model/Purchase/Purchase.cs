namespace EasyAccountingAPI.Model.Purchase
{
    [Table("Purchases", Schema = "Purchase")]
    public class Purchase : IAuditableEntity, IDelatableEntity
    {
        public Purchase()
        {
            PurchaseItems = new HashSet<PurchaseItem>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide order number.")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Please, provide company.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please, provide purchase date.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Please, provide payment date.")]
        public DateTime PaymentDate { get; set; }
        
        [Required(ErrorMessage = "Please, provide vendor.")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Please, provide total amount.")]
        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Company Company { get; set; }
        public Vendor Vendor { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}