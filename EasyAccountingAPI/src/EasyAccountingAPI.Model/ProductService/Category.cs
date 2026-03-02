namespace EasyAccountingAPI.Model.ProductService
{
    [Table("Categories", Schema = "ProductService")]
    public class Category : IAuditableEntity, IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public int CompanyId { get; set; }

        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Company Company { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }
    }
}