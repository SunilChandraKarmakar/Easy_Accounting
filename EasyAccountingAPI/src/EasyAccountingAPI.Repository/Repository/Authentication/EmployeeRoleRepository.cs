namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class EmployeeRoleRepository : BaseRepository<EmployeeRole>, IEmployeeRoleRepository
    {
        public EmployeeRoleRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}