namespace EasyAccountingAPI.Model.MasterSettings.VendorInformation
{
    [Table("Vendors", Schema = "MasterSettings")]
    public class Vendor : IAuditableEntity, IDelatableEntity
    {
        public Vendor()
        {
            VendorAddresses = new HashSet<VendorAddress>();
            Purchases = new HashSet<Purchase.Purchase>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Business Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Business name must be 2 to 50 character.")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be 2 to 100 character.")]
        public string  FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(100, MinimumLength = 11, ErrorMessage = "Email must be 11 to 100 character.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? Phone { get; set; }
        public string? Image { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Company Company { get; set; }
        public ICollection<VendorAddress> VendorAddresses { get; set; }
        public ICollection<Purchase.Purchase> Purchases { get; set; }
    }
}