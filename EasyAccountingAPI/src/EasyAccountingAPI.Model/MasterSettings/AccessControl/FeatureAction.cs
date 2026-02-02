namespace EasyAccountingAPI.Model.MasterSettings.AccessControl
{
    [Table("FeatureActions", Schema = "MasterSettings")]
    public class FeatureAction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Feature is required.")]
        public int FeatureId { get; set; }

        [Required(ErrorMessage = "Action is required.")]
        public int ActionId { get; set; }

        public Feature Feature { get; set; }    
        public Action Action { get; set; }
    }
}