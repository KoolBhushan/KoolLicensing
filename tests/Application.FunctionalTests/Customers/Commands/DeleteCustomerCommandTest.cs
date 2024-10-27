using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Customers.Commands.CreateCustomer;
using KoolLicensing.Application.Customers.Commands.DeleteCustomer;
using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using static KoolLicensing.Application.FunctionalTests.Testing;

namespace KoolLicensing.Application.FunctionalTests.Customers.Commands;

public class DeleteCustomerCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidCustomerId()
    {
        var command = new DeleteCustomerCommand(99);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<EntityNotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteCustomer()
    {
        var userId = await RunAsDefaultUserAsync();

        var customerId = await SendAsync(new CreateCustomerCommand
        {
            Name = "Test Customer",
            Email = "test@customer",
            CompanyName = "Test Company"
        });

        await SendAsync(new DeleteCustomerCommand(customerId));

        var item = await FindAsync<Product>(customerId);

        item.Should().BeNull();
    }
}
