namespace EasyAccountingAPI.Application.ApplicationLogics.AuthenticationLogic.Queries
{
    public class GetUsersQuery : FilterPageModel, IRequest<ICollection<UserModel>>
    {
        public class Handler : IRequestHandler<GetUsersQuery, ICollection<UserModel>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<ICollection<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                // Step 1: Get all users who need password reset and are linked with employees
                var existUsers = _userManager.Users
                    .AsNoTracking()
                    .AsQueryable();

                // Apply filtering
                if (request.FilterValue is not null)
                    query = query.Where(filterExpression);

                // Apply sorting
                if (!string.IsNullOrWhiteSpace(model.SortColumn) && sortableColumns is not null
                    && sortableColumns.TryGetValue(model.SortColumn.ToLower(), out var sortExpression))
                    query = model.SortOrder?.ToLower() == "descend" ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);
                else if (defaultSortExpression is not null)
                    query = query.OrderBy(defaultSortExpression);

                int totalCount = await query.CountAsync(cancellationToken);

                var items = await query
                    .Skip(model.PageIndex * model.PageSize)
                    .Take(model.PageSize)
                    .ToListAsync(cancellationToken);

                return new FilterPageResultModel<T>(items, totalCount);

                var mapUser = _mapper.Map<ICollection<UserModel>>(existUsers);
                return mapUser;
            }
        }
    }
}