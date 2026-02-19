namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Model
{
    public class EmployeeFeatureActionViewModel
    {
        public EmployeeFeatureActionCreateModel CreateModel { get; set; }
        public EmployeeFeatureActionGridModel GridModel { get; set; }
    }

    public class EmployeeFeatureActionCreateModel
    {
        public int EmployeeId { get; set; }
        public int FeatureId { get; set; }
        public int[] ActionIds { get; set; }
    }

    public class EmployeeFeatureActionGridModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<FeatureWithActionsModel> Features { get; set; }
    }

    public class FeatureWithActionsModel
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public List<ActionDetails> Actions { get; set; }
    }

    public class ActionDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}