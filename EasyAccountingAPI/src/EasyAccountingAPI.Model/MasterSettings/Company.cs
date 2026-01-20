namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("Companies", Schema = "MasterSettings")]
    public class Company : IAuditableEntity, IDelatableEntity
    {
        public Company()
        {
            InvoiceSettings = new HashSet<InvoiceSetting>();
        }

        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Email { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(30, MinimumLength = 11)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please, provide country.")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please, provide city.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please, provide currency.")]
        public int CurrencyId { get; set; }

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
    }
}