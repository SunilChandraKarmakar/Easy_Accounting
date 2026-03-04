namespace EasyAccountingAPI.Model.ProductService
{

    [Table("Variations", Schema = "ProductService")]
    public class Variation : IAuditableEntity, IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Variation name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Variation name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Variation values are required.")]
        public string[] Values { get; set; }

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