using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Application.Common.Exceptions;
using static KoolLicensing.Application.FunctionalTests.Testing;

namespace KoolLicensing.Application.FunctionalTests.Products.Commands;

public class UpdateProductCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new UpdateProductCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task ShouldRequireValidProduct()
    {
        var userId = await RunAsDefaultUserAsync();
        var commad = new UpdateProductCommand { Id = 99, Name = "Test Product", Description = "Test Description" };
        await FluentActions.Invoking(() => SendAsync(commad)).Should().ThrowAsync<EntityNotFoundException>();
    }
    
    [Test]
    public async Task ShouldUpdateProduct()
    {
        var userId = await RunAsDefaultUserAsync();

        var productId = await SendAsync(new CreateProductCommand
        {
            Name = "Test Product",
            Description = "Test Description"
        });
        
        var command = new UpdateProductCommand { Id = productId, Name = "Updated Product", Description = "Updated Description" };
        await SendAsync(command);
        
        var item = await FindAsync<Product>(productId);

        item.Should().NotBeNull();
        item!.ProductCode.Should().NotBeNull();
        item.Name.Should().Be("Updated Product");
        item.Description.Should().Be("Updated Description");
    }
}
