using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Application.Common.Exceptions;
using static KoolLicensing.Application.FunctionalTests.Testing;
using KoolLicensing.Application.Customers.Commands.UpdateCustomer;
using KoolLicensing.Application.Customers.Commands.CreateCustomer;

namespace KoolLicensing.Application.FunctionalTests.Customers.Commands;

public class UpdateCustomerCommandTest
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new UpdateCustomerCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidCustomer()
    {
        var userId = await RunAsDefaultUserAsync();
        var commad = new UpdateCustomerCommand
        {
            Name = "Test Customer",
            Email = "test@customer",
            CompanyName = "Test Company",
            Id = 99            
        };
        
        await FluentActions.Invoking(() => SendAsync(commad)).Should().ThrowAsync<EntityNotFoundException>();
    }

    [Test]
    public async Task ShouldUpdateCustomer()
    {
        var userId = await RunAsDefaultUserAsync();

        var customerId = await SendAsync(new CreateCustomerCommand
        {
            Name = "Test Customer",
            Email = "test@customer",
            CompanyName = "Test Company"
        });

        var command = new UpdateCustomerCommand { Id = customerId, Name = "Updated Customer", Email = "updated@customer", CompanyName = "Updated Company" };
        await SendAsync(command);

        var item = await FindAsync<Customer>(customerId);

        item.Should().NotBeNull();
        item!.Name.Should().Be("Updated Customer");
        item.Email.Should().Be("updated@customer");
        item.CompanyName.Should().Be("Updated Company");
    }
}
