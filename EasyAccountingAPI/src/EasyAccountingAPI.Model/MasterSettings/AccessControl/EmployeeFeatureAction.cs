namespace EasyAccountingAPI.Model.MasterSettings.AccessControl
{
    [Table("EmployeeFeatureActions", Schema = "MasterSettings")]
    public class EmployeeFeatureAction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide the employee.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please, provide feature.")]
        public int FeatureId { get; set; }

        [Required(ErrorMessage = "Please, provide action.")]
        public int ActionId { get; set; }

        public Employee Employee { get; set; }
        public Feature Feature { get; set; }
        public Action Action { get; set; }
    }
}