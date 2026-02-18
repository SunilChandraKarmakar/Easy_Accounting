namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Model
{
    public class EmployeeFeatureActionViewModel
    {
        public EmployeeFeatureActionCreateModel CreateModel { get; set; }
    }

    public class EmployeeFeatureActionCreateModel
    {
        public int EmployeeId { get; set; }
        public int FeatureId { get; set; }
        public int[] ActionIds { get; set; }
    }
}