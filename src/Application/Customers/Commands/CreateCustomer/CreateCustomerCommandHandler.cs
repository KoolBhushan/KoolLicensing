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

using KoolLicensing.Application.Common.Interfaces;
using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Domain.Events;
using Microsoft.Extensions.Logging;

namespace KoolLicensing.Application.Customers.Commands.CreateCustomer;
public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ILogger<CreateCustomerCommandHandler> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly IUser _user;

    public CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger, IApplicationDbContext dbContext, IUser user)
    {
        _logger = logger;
        _dbContext = dbContext;
        _user = user;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            UserId = _user.Id!,
            CompanyName = request.CompanyName,
            Products = new List<Product>(),
            Licenses = new List<License>()
        };

        customer.AddDomainEvent(new CustomerCreatedEvent(customer));

        _dbContext.Customers.Add(customer);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
