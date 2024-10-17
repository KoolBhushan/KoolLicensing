// Author:
// Bhushan Kamble
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.ComponentModel;
using KoolLicensing.Application.Common.Interfaces;
using KoolLicensing.Application.Common.Security;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoolLicensing.Application.Products.Commands;
public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICryptoService _cryptoService;
    private readonly IUser _user;

    public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IApplicationDbContext dbContext, ICryptoService cryptoService, IUser user)
    {
        _logger = logger;
        _dbContext = dbContext;
        _cryptoService = cryptoService;
        _user = user;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productCode = _cryptoService.GenerateProductCode();

        var rsa = new NewRSA();

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            UserId = _user.Id!,
            ProductCode = productCode,
            PrivateKey = rsa.ExportEncryptedPrivateKey(2, productCode),
            PublicKey = rsa.ExportPublicKey(),
            Customers = new List<Customer>(),
            Licenses = new List<Domain.Entities.License>()
        };

        product.AddDomainEvent(new ProductCreatedEvent(product));

        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
