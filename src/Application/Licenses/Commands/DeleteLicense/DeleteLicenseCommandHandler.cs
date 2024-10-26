using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace KoolLicensing.Application.Licenses.Commands.DeleteLicense;
public sealed class DeleteLicenseCommandHandler : IRequestHandler<DeleteLicenseCommand>
{
    private readonly ILogger<DeleteLicenseCommandHandler> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly IUser _user;

    public DeleteLicenseCommandHandler(ILogger<DeleteLicenseCommandHandler> logger, IApplicationDbContext dbContext, IUser user)
    {
        _logger = logger;
        _dbContext = dbContext;
        _user = user;
    }

    public async Task Handle(DeleteLicenseCommand request, CancellationToken cancellationToken)
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

        _dbContext.Licenses.Remove(license);
        product.Licenses.Remove(license);
        product.Customers.Remove(license.Customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
