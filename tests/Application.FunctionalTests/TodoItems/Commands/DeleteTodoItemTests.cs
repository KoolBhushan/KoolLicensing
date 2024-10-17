using KoolLicensing.Application.TodoItems.Commands.CreateTodoItem;
using KoolLicensing.Application.TodoItems.Commands.DeleteTodoItem;
using KoolLicensing.Application.TodoLists.Commands.CreateTodoList;
using KoolLicensing.Domain.Entities;

namespace KoolLicensing.Application.FunctionalTests.TodoItems.Commands;

using static Testing;

public class DeleteTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand(99);

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        await SendAsync(new DeleteTodoItemCommand(itemId));

        var item = await FindAsync<License>(itemId);

        item.Should().BeNull();
    }
}
