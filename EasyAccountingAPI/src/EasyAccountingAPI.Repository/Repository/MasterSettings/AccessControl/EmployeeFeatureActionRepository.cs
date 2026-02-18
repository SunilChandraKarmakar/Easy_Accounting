namespace EasyAccountingAPI.Repository.Repository.MasterSettings.AccessControl
{
    public class EmployeeFeatureActionRepository : BaseRepository<EmployeeFeatureAction>, IEmployeeFeatureActionRepository
    {
        public EmployeeFeatureActionRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}