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

using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace KoolLicensing.Application.Customers.Commands.UpdateCustomer;
public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly IUser _user;

    public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger, IApplicationDbContext dbContext, IUser user)
    {
        _logger = logger;
        _dbContext = dbContext;
        _user = user;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.Where(x => x.UserId.Equals(_user.Id!)).FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

        if (customer == null)
        {
            var error = $"The customer with id {request.Id} does not exist.";
            _logger.LogCritical(error);
            throw new EntityNotFoundException(error);
        }

        customer.Name = request.Name;
        customer.CompanyName = request.CompanyName;
        customer.Email = request.Email;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
