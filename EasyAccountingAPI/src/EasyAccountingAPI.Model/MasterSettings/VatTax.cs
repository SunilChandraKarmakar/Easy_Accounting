namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("VatTaxes", Schema = "MasterSettings")]
    public class VatTax : IAuditableEntity, IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tax Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string TaxName { get; set; }

        [Required(ErrorMessage = "Tax Rate is required.")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? TaxNumber { get; set; }
        public string? Description { get; set; }

        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Company Company { get; set; }
    }
}