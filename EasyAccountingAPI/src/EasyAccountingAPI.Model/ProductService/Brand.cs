namespace EasyAccountingAPI.Model.ProductService
{
    [Table("Brands", Schema = "ProductService")]
    public class Brand : IAuditableEntity, IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Company Company { get; set; }
    }
}