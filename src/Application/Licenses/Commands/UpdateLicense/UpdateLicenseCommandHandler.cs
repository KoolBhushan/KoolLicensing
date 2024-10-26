using AutoMapper.Features;
using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Interfaces;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KoolLicensing.Application.Licenses.Commands.UpdateLicense;
public sealed class UpdateLicenseCommandHandler : IRequestHandler<UpdateLicenseCommand, LicenseResponse>
{
    private readonly ILogger<UpdateLicenseCommandHandler> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly IUser _user;
    private readonly IMapper _mapper;

    public UpdateLicenseCommandHandler(ILogger<UpdateLicenseCommandHandler> logger, IApplicationDbContext dbContext, IUser user, IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _user = user;
        _mapper = mapper;
    }

    public async Task<LicenseResponse> Handle(UpdateLicenseCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
                .Where(x => x.UserId == _user.Id)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.ProductId));

        if (product == null)
        {
            var error = $"The product with id {request.ProductId} does not exist.";
            _logger.LogError(error);
            throw new EntityNotFoundException(error);
        }

        var license = await _dbContext.Licenses
            .Where(x => x.ProductId == request.ProductId)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.LicenseId));

        if (license == null)
        {
            var error = $"The license with id {request.LicenseId} does not exist.";
            _logger.LogError(error);
            throw new EntityNotFoundException(error);
        }

        license.MaxNoOfMachines = request.MaxNoOfMachines;
        license.Feature1 = request.Feature1;
        license.Feature2 = request.Feature2;
        license.Feature3 = request.Feature3;
        license.Feature4 = request.Feature4;
        license.Feature5 = request.Feature5;
        license.Feature6 = request.Feature6;
        license.Feature7 = request.Feature7;
        license.Feature8 = request.Feature8;
        license.Feature9 = request.Feature9;
        license.LicenseType = request.LicenseType;
        license.Expires = license.Expires.AddDays(request.ValidityInDays);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LicenseResponse>(license);
    }
}
