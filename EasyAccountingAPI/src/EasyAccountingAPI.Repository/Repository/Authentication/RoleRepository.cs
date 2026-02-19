namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            var roles = await db.Roles
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return roles;
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            var role = await db.Roles
                .Where(r => r.Name == roleName)
                .FirstOrDefaultAsync(cancellationToken);

            return role;
        }

        public async Task<List<string>> GetRoleNamesAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken)
        {
            return await db.Roles
                .Where(r => roleNames.Contains(r.Name))
                .Select(r => r.Name)
                .ToListAsync(cancellationToken);
        }
    }
}