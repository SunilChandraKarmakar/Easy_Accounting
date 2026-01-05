namespace EasyAccountingAPI.Application.ApplicationLogics.AuthenticationLogic.Queries
{
    public class GetUsersQuery : FilterPageModel, IRequest<FilterPageResultModel<UserModel>>
    {
        public class Handler : IRequestHandler<GetUsersQuery, FilterPageResultModel<UserModel>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                // Get all users
                var query = _userManager.Users.AsNoTracking();

                // Apply filtering
                var filter = request.FilterValue?.Trim();
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    // SQL LIKE: %filter%
                    var pattern = $"%{filter}%";
                    query = query.Where(u => (u.FullName != null && EF.Functions.Like(u.FullName, pattern)) 
                        || (u.Email != null && EF.Functions.Like(u.Email, pattern)));
                }

                // Apply sorting
                var sortableColumns = new Dictionary<string, Expression<Func<User, object>>>(StringComparer.OrdinalIgnoreCase)
                {
                    ["name"] = u => u.FullName!,
                    ["email"] = u => u.Email!,
                    ["id"] = u => u.Id
                };

                var sortColumn = request.SortColumn?.Trim();
                var sortOrder = request.SortOrder?.Trim();

                if (!string.IsNullOrWhiteSpace(sortColumn) && sortableColumns.TryGetValue(sortColumn, out var sortExpression))
                    query = string.Equals(sortOrder, "descend", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(sortExpression)
                        : query.OrderBy(sortExpression);
                else
                    query = query.OrderBy(u => u.Id);

                // Get total count before paging
                var totalCount = await query.CountAsync(cancellationToken);

                // Apply paging
                var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
                var pageIndex = request.PageIndex < 0 ? 0 : request.PageIndex;

                // Retrieve paged result
                var result = await query
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return new FilterPageResultModel<UserModel>(result, totalCount);
            }
        }
    }
}