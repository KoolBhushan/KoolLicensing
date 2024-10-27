using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Application.Common.Exceptions;
using static KoolLicensing.Application.FunctionalTests.Testing;
using KoolLicensing.Application.Licenses.Commands.UpdateLicense;
using KoolLicensing.Application.Customers.Commands.CreateCustomer;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;

namespace KoolLicensing.Application.FunctionalTests.Licenses.Commands;

public class UpdateLicenseCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new UpdateLicenseCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidLicense()
    {
        var userId = await RunAsDefaultUserAsync();
        var commad = new UpdateLicenseCommand { ProductId = 99, LicenseId = 99, LicenseType = Domain.Enums.LicenseType.Perpetual, MaxNoOfMachines = 2, ValidityInDays = 45 };
        await FluentActions.Invoking(() => SendAsync(commad)).Should().ThrowAsync<EntityNotFoundException>();
    }

    [Test]
    public async Task ShouldUpdateLicense()
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
            Feature9 = false
        };

        var response = await SendAsync(command);
        var updateCommand = new UpdateLicenseCommand
        {
            ProductId = productId,
            LicenseId = 1,
            LicenseType = Domain.Enums.LicenseType.Subscription,
            ValidityInDays = 60,
            MaxNoOfMachines = 1,
            Feature1 = true,
            Feature2 = true,
            Feature3 = true,
            Feature4 = true,
            Feature5 = true,
            Feature6 = true,
            Feature7 = true,
            Feature8 = true,
            Feature9 = true
        };

        var updateResponse = await SendAsync(updateCommand);

        var item = await FindAsync<License>(1);

        item.Should().NotBeNull();
        item!.LicenseType.Should().Be(Domain.Enums.LicenseType.Subscription);
        item.Expires.Should().BeCloseTo(DateTimeOffset.Now.AddDays(60), TimeSpan.FromMilliseconds(10000));
        item.MaxNoOfMachines.Should().Be(1);
        item.CustomerId.Should().Be(customerId);
        item.Feature1.Should().Be(true);
        item.Feature2.Should().Be(true);
        item.Feature3.Should().Be(true);
        item.Feature4.Should().Be(true);
        item.Feature5.Should().Be(true);
        item.Feature6.Should().Be(true);
        item.Feature7.Should().Be(true);
        item.Feature8.Should().Be(true);
        item.Feature9.Should().Be(true);
    }
}
