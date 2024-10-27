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
using KoolLicensing.Application.Customers.Commands.CreateCustomer;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;
using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using NUnit.Framework.Internal;
using static KoolLicensing.Application.FunctionalTests.Testing;

namespace KoolLicensing.Application.FunctionalTests.Licenses.Commands;

public class CreateLicenseCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateLicenseCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateLicense()
    {
        var userId = await RunAsDefaultUserAsync();

        var productId = await SendAsync(new CreateProductCommand
        {
            Name = "Test Product",
            Description = "Test Description"
        });

        var customerId = await SendAsync(new CreateCustomerCommand
        {
            Name = "Test Customer",
            Email = "test@customer",
            CompanyName = "Test Company"
        });

        var command = new CreateLicenseCommand
        {
            ProductId = productId,
            CustomerId = customerId,
            LicenseType = Domain.Enums.LicenseType.Subscription,
            ValidityInDays = 90,
            MaxNoOfMachines = 2,
            Feature1 = false,
            Feature2 = true,
            Feature3 = false,
            Feature4 = true,
            Feature5 = false,
            Feature6= true,
            Feature7 = false,
            Feature8 = true,
            Feature9 = false,
        };

        var response = await SendAsync(command);

        var item = await FindAsync<License>(1);

        item.Should().NotBeNull();
        item!.LicenseType.Should().Be(response.LicenseType);
        item.Key!.Value.Should().Be(response.Key);
        item.Expires.Should().BeCloseTo(DateTimeOffset.Now.AddDays(90), TimeSpan.FromMilliseconds(10000));
        item.Product.Name.Should().Be(response.ProductName);
        item.Customer.Name.Should().Be(response.CustomerName);
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
