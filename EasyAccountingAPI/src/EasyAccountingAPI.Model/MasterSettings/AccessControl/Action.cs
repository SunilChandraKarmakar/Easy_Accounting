namespace EasyAccountingAPI.Model.MasterSettings.AccessControl
{
    [Table("Actions", Schema = "MasterSettings")]
    public class Action : IDelatableEntity
    {
        public Action()
        {
            FeatureActions = new HashSet<FeatureAction>();
            EmployeeFeatureActions = new HashSet<EmployeeFeatureAction>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public ICollection<FeatureAction> FeatureActions { get; set; }
        public ICollection<EmployeeFeatureAction> EmployeeFeatureActions { get; set; }
    }
}