namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class EmployeeRoleRepository : BaseRepository<EmployeeRole>, IEmployeeRoleRepository
    {
        public EmployeeRoleRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<string> GetEmployeeRoleNameByEmployeeId(int employeeId)
        {
            // Get employee role name
            var employeeRoleName = await db.EmployeeRoles
                .AsNoTracking()
                .Include(er => er.Role)
                .Where(er => er.EmployeeId == employeeId)
                .Select(s => s.Role.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(employeeRoleName) || string.IsNullOrWhiteSpace(employeeRoleName))
                return "This user does not currently have an assigned role.";

            return employeeRoleName;
        }

        public async Task<EmployeeRole?> GetEmployeeRoleByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var emoloyeeRole = await db.EmployeeRoles
                .Where(er => er.EmployeeId == employeeId)
                .FirstOrDefaultAsync(cancellationToken);

            return emoloyeeRole;
        }
    }
}