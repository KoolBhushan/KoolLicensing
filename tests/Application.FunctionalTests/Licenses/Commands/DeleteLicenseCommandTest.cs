using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Customers.Commands.CreateCustomer;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;
using KoolLicensing.Application.Licenses.Commands.DeleteLicense;
using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using static KoolLicensing.Application.FunctionalTests.Testing;

namespace KoolLicensing.Application.FunctionalTests.Licenses.Commands;

public class DeleteLicenseCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidLicenseId()
    {
        var command = new DeleteLicenseCommand(99, 99);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<EntityNotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteLicense()
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
            Feature6 = true,
            Feature7 = false,
            Feature8 = true,
            Feature9 = false,
        };


        var response = await SendAsync(command);

        await SendAsync(new DeleteLicenseCommand(productId, 1));

        var productItem = await FindAsync<Product>(productId);
        var licenseItem1 = await FindAsync<License>(1);
        var licenseItem2 = productItem!.Licenses.FirstOrDefault(x => x.Id.Equals(1));
        var customerItem = productItem!.Customers.FirstOrDefault(x => x.Id.Equals(customerId));

        licenseItem1.Should().BeNull();
        licenseItem2.Should().BeNull();
        customerItem.Should().BeNull();
    }
}
