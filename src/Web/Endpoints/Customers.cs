using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Models;
using KoolLicensing.Application.Customers.Commands.CreateCustomer;
using KoolLicensing.Application.Customers.Commands.DeleteCustomer;
using KoolLicensing.Application.Customers.Commands.UpdateCustomer;
using KoolLicensing.Application.Customers.Queries;
using KoolLicensing.Application.Customers.Queries.GetCustomer;
using KoolLicensing.Application.Customers.Queries.GetCustomersWithPagination;

namespace KoolLicensing.Web.Endpoints;

public class Customers : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetCustomersWithPaginationAsync)
            .MapGet(GetCustomerAsync, "{id}")
            .MapDelete(DeleteCustomerAsync, "{id}")
            .MapPut(UpdateCustomerAsync, "{id}")
            .MapPost(CreateCustomerAsync);
    }

    public async Task<IResult> GetCustomerAsync(ISender sender, int id)
    {
        try
        {
            var resposne = await sender.Send(new GetCustomerQuery(id));
            return Results.Ok(resposne);
        }
        catch (EntityNotFoundException ex)
        {
            return Results.NotFound(ex);
        }
    }

    public async Task<PaginatedList<CustomerResponse>> GetCustomersWithPaginationAsync(ISender sender, [AsParameters] GetCustomersWithPaginationQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<IResult> CreateCustomerAsync(ISender sender, CreateCustomerCommand command)
    {
        await sender.Send(command);
        return Results.Created();
    }

    public async Task<IResult> UpdateCustomerAsync(ISender sender, int id, UpdateCustomerCommand command)
    {
        if (id != command.Id) return Results.BadRequest();

        try
        {
            await sender.Send(command);
            return Results.NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return Results.NotFound(ex);
        }
    }

    public async Task<IResult> DeleteCustomerAsync(ISender sender, int id)
    {
        try
        {
            await sender.Send(new DeleteCustomerCommand(id));
            return Results.NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return Results.NotFound(ex);
        }
    }
}
