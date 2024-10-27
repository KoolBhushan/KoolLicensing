using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Domain.Entities;
using static KoolLicensing.Application.FunctionalTests.Testing;

namespace KoolLicensing.Application.FunctionalTests.Products.Commands;

public class DeleteProductCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidProductId()
    {
        var command = new DeleteProductCommand{ Id = 99 };
            
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<EntityNotFoundException>();
    }
    
    [Test]
    public async Task ShouldDeleteProduct()
    {
        var userId = await RunAsDefaultUserAsync();

        var createProductCommand = new CreateProductCommand
        {
            Name = "Test Product",
            Description = "Test Description"
        };

        var itemId = await SendAsync(createProductCommand);
        await SendAsync(new DeleteProductCommand{ Id = itemId });

        var item = await FindAsync<Product>(itemId);

        item.Should().BeNull();
    }
}
