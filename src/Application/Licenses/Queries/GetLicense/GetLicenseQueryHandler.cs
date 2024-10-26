using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Interfaces;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;
using MediatR;

namespace KoolLicensing.Application.Licenses.Queries.GetLicense;
public sealed class GetLicenseQueryHandler : IRequestHandler<GetLicenseQuery, LicenseResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetLicenseQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    public async Task<LicenseResponse> Handle(GetLicenseQuery request, CancellationToken cancellationToken)
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

        var license = await _context.Licenses
            .AsNoTracking()
            .Where(x => x.ProductId == request.ProductId)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.LicenseId));

       

        if (license == null)
        {
            var error = $"The license with id {request.LicenseId} does not exist.";
            throw new EntityNotFoundException(error);
        }

        return _mapper.Map<LicenseResponse>(license);
    }
}
