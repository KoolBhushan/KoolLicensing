using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Interfaces;
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

    public CreateLicenseCommandHandler(ILogger<CreateLicenseCommandHandler> logger, IApplicationDbContext dbContext, ICryptoService cryptoService, IUser user)
    {
        _logger = logger;
        _dbContext = dbContext;
        _cryptoService = cryptoService;
        _user = user;
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
        var newLicense = new License
        {
            Key = keyInfo,
            LicenseType = request.LicenseType,
            Expires = DateTime.UtcNow.AddDays(request.ValidityInDays),
            Feature0 = true,
            Feature1 = request.Feature1,
            Feature2 = request.Feature2,
            Feature3 = request.Feature3,
            Feature4 = request.Feature4,
            Feature5 = request.Feature5,
            Feature6 = request.Feature6,
            Feature7 = request.Feature7,
            Feature8 = request.Feature8,
            Feature9 = request.Feature9,
            Block = false,
            TrialActivation = request.LicenseType == LicenseType.Trial,
            MaxNoOfMachines = request.MaxNoOfMachines,
            Product = product,
            Customer = customer,
            ProductId = request.ProductId,
            CustomerId = request.CustomerId,
            ActivatedMachines = new List<Machine>()
        };

        await _dbContext.Licenses.AddAsync(newLicense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new LicenseResponse
        {
            Key = newLicense.Key.Value,
            LicenseType = newLicense.LicenseType,
            ProductName = product.Name,
            CustomerName = customer.Name,
            Expires = newLicense.Expires,
        };
    }
}
