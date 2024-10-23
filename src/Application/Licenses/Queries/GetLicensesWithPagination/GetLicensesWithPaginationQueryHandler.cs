using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Interfaces;
using KoolLicensing.Application.Common.Mappings;
using KoolLicensing.Application.Common.Models;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;

namespace KoolLicensing.Application.Licenses.Queries.GetLicensesWithPagination;
public sealed class GetLicensesWithPaginationQueryHandler : IRequestHandler<GetLicensesWithPaginationQuery, PaginatedList<LicenseResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetLicensesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    public async Task<PaginatedList<LicenseResponse>> Handle(GetLicensesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
                .AsNoTracking()
                .Where(x => x.UserId == _user.Id)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.ProductId));

        if (product == null)
        {
            var error = $"The product with id {request.ProductId} does not exist.";
            throw new EntityNotFoundException(error);
        }

        var licenses = _context.Licenses
            .AsNoTracking()
            .Where(x => x.ProductId == request.ProductId)
            .OrderBy(x => x.Id)
            .Select(x => new LicenseResponse { Key = x.Key!.Value, LicenseType = x.LicenseType, Expires = x.Expires, ProductName = x.Product.Name, CustomerName = x.Customer.Name })
            .AsQueryable();

        return await PaginatedList<LicenseResponse>.CreateAsync(licenses, request.PageNumber, request.PageSize);
    }
}
