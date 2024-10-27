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

using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Application.Common.Exceptions;
using static KoolLicensing.Application.FunctionalTests.Testing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using KoolLicensing.Application.Customers.Commands.CreateCustomer;
using NUnit.Framework.Internal;

namespace KoolLicensing.Application.FunctionalTests.Customers.Commands;

public class CreateCustomerCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateCustomerCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateCustomer()
    {
        var userId = await RunAsDefaultUserAsync();

        var customerId = await SendAsync(new CreateCustomerCommand
        {
            Name = "Test Customer",
            Email = "test@customer",
            CompanyName = "Test Company"
        });

        var item = await FindAsync<Customer>(customerId);

        item.Should().NotBeNull();
        item!.Name.Should().Be("Test Customer");
        item.Email.Should().Be("test@customer");
        item.CompanyName.Should().Be("Test Company");
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
