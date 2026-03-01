namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("Companies", Schema = "MasterSettings")]
    public class Company : IAuditableEntity, IDelatableEntity
    {
        public Company()
        {
            InvoiceSettings = new HashSet<InvoiceSetting>();
            Employees = new HashSet<Employee>();
            VatTaxes = new HashSet<VatTax>();
            Brands = new HashSet<Brand>();
        }

        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Email { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [StringLength(30, MinimumLength = 11)]
        public string? Phone { get; set; }

        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? CurrencyId { get; set; }
        public string? Logo { get; set; }
        public string? TaxNo { get; set; }
        public bool IsSellWithPos { get; set; }
        public bool IsProductHaveBrand { get; set; }
        public bool IsDefaultCompany { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Address { get; set; }

        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Country Country { get; set; }
        public City City { get; set; }
        public Currency Currency { get; set; }
        public ICollection<InvoiceSetting> InvoiceSettings { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<VatTax> VatTaxes { get; set; }
        public ICollection<Brand> Brands { get; set; }
    }
}