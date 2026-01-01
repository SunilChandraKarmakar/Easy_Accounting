namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}