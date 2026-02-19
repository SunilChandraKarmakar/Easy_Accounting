namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.RoleLogic.Command
{
    public class CreateRoleSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateRoleSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IRoleRepository _roleRepository;

            public Handler(
                IUnitOfWorkRepository unitOfWork,
                IRoleRepository roleRepository)
            {
                _unitOfWork = unitOfWork;
                _roleRepository = roleRepository;
            }

            public async Task<bool> Handle(CreateRoleSeedCommand request, CancellationToken cancellationToken)
            {
                var roleNames = GetRoles();

                // Only fetch roles that match required names
                var existingRoleNames = await _roleRepository.GetRoleNamesAsync(roleNames, cancellationToken);
                var existingRoleSet = new HashSet<string>(existingRoleNames, StringComparer.OrdinalIgnoreCase);

                // Filter only missing roles
                var rolesToCreate = roleNames
                    .Where(role => !existingRoleSet.Contains(role))
                    .Select(role => new Role
                    {
                        Name = role,
                        Description = string.Empty
                    })
                    .ToList();

                // If nothing to insert → exit early
                if (!rolesToCreate.Any())
                    return true;

                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    await _roleRepository.BulkCreateAsync(rolesToCreate, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }

            private static IReadOnlyList<string> GetRoles() => new List<string>
            {
                "Super Admin",
                "Admin",
                "Employee"
            };
        }
    }
}