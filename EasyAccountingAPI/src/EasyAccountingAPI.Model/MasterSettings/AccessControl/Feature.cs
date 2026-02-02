namespace EasyAccountingAPI.Model.MasterSettings.AccessControl
{
    [Table("Features", Schema = "MasterSettings")]
    public class Feature : IDelatableEntity
    {
        public Feature()
        {
            FeatureActions = new HashSet<FeatureAction>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [StringLength(20, MinimumLength = 2)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }        

        [Required(ErrorMessage = "Module is required.")]
        public int ModuleId { get; set; }

        public string? ControllerName { get; set; }
        public string? TableName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Module Module { get; set; }
        public ICollection<FeatureAction> FeatureActions { get; set; }
    }
}