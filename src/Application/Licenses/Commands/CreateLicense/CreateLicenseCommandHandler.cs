using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Interfaces;
using KoolLicensing.Domain.Builders;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KoolLicensing.Application.Licenses.Commands.CreateLicense;
internal sealed class CreateLicenseCommandHandler : IRequestHandler<CreateLicenseCommand, LicenseResponse>
{
    private readonly ILogger<CreateLicenseCommandHandler> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICryptoService _cryptoService;
    private readonly IUser _user;
    private readonly ILicenseBuilder _licenseBuilder;

    public CreateLicenseCommandHandler(ILogger<CreateLicenseCommandHandler> logger, IApplicationDbContext dbContext, ICryptoService cryptoService, IUser user, ILicenseBuilder licenseBuilder)
    {
        _logger = logger;
        _dbContext = dbContext;
        _cryptoService = cryptoService;
        _user = user;
        _licenseBuilder = licenseBuilder;
    }

    public async Task<LicenseResponse> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.Where(x => x.UserId.Equals(_user.Id!)).FirstOrDefaultAsync(x => x.Id.Equals(request.ProductId));

        if (product == null)
        {
            var error = $"The product with id {request.ProductId} does not exist.";
            _logger.LogCritical(error);
            throw new EntityNotFoundException(error);
        }

        var customer = await _dbContext.Customers.Where(x => x.UserId.Equals(_user.Id!)).FirstOrDefaultAsync(x => x.Id.Equals(request.CustomerId));

        if (customer == null)
        {
            var error = $"The customer with id {request.CustomerId} does not exist.";
            _logger.LogCritical(error);
            throw new EntityNotFoundException(error);
        }

        product.Customers.Add(customer);

        var keyInfo = _cryptoService.GenerateKey();

        var newLicense = _licenseBuilder
            .OfType(request.LicenseType)
            .WithKey(keyInfo)
            .AddValidDays(request.ValidityInDays)
            .WithMaxMachines(request.MaxNoOfMachines)
            .ForCustomer(customer)
            .ForProduct(product)
            .WithFeature1(request.Feature1)
            .WithFeature2(request.Feature2)
            .WithFeature3(request.Feature3)
            .WithFeature4(request.Feature4)
            .WithFeature5(request.Feature5)
            .WithFeature6(request.Feature6)
            .WithFeature7(request.Feature7)
            .WithFeature8(request.Feature8)
            .WithFeature9(request.Feature9)
            .Build();

        await _dbContext.Licenses.AddAsync(newLicense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new LicenseResponse
        {
            Key = newLicense.Key!.Value,
            LicenseType = newLicense.LicenseType,
            ProductName = product.Name,
            CustomerName = customer.Name,
            Expires = newLicense.Expires,
        };
    }
}
