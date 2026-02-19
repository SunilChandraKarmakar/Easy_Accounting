namespace EasyAccountingAPI.Repository.Contracts.Authentication
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken);
        Task<List<string>> GetRoleNamesAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken);
    }
}