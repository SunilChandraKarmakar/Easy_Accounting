namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("InvoiceSettings", Schema = "MasterSettings")]
    public class InvoiceSetting : IAuditableEntity, IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide invoice due date count.")]
        public int InvoiceDueDateCount { get; set; }

        public string? InvoiceColor { get; set; }
        public string? InvoiceFooter { get; set; }
        public string? InvoiceTerms { get; set; }
        public bool IsHideInvoiceHeader { get; set; }
        public bool IsShowPreviousDue { get; set; }
        public bool IsShowInvoiceCreatorName { get; set; }
        public bool IsShowCustomerSignature { get; set; }
        public bool IsCreateInvoiceWithoutPurchase { get; set; }
        public bool IsServiceProviderAttributionUnderInvoice { get; set; }
        public bool IsDefaultInvoiceSetting { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}