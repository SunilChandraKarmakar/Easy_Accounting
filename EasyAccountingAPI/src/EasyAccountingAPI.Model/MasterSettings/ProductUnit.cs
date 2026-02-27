namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("ProductUnits", Schema = "MasterSettings")]
    public class ProductUnit : IAuditableEntity, IDelatableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Unit name is required.")]
        [StringLength(50, ErrorMessage = "Unit name cannot exceed 50 characters.")]
        public string Name { get; set; }

        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}