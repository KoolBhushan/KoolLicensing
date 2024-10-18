
using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Models;
using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Application.Products.Queries;
using KoolLicensing.Application.Products.Queries.GetProduct;
using KoolLicensing.Application.Products.Queries.GetProductsWithPagination;

namespace KoolLicensing.Web.Endpoints;

public class Products : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetProductsWithPaginationAsync)
            .MapGet(GetProductAsync, "{id}")
            .MapDelete(DeleteProductAsync, "{id}")
            .MapPut(UpdateProductAsync, "{id}")
            .MapPost(CreateProductAsync);
    }

    public async Task<IResult> GetProductAsync(ISender sender, int id)
    {
        try
        {
            var resposne = await sender.Send(new GetProductQuery { Id = id });
            return Results.Ok(resposne);
        }
        catch (EntityNotFoundException ex)
        {
            return Results.NotFound(ex);
        }
    }

    public async Task<PaginatedList<ProductResponse>> GetProductsWithPaginationAsync(ISender sender, [AsParameters] GetProductsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<IResult> CreateProductAsync(ISender sender, CreateProductCommand command)
    {
        var response = await sender.Send(command);
        return Results.Created();
    }

    public async Task<IResult> UpdateProductAsync(ISender sender, int id, UpdateProductCommand command)
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

    public async Task<IResult> DeleteProductAsync(ISender sender, int id)
    {
        try
        {
            await sender.Send(new DeleteProductCommand { Id = id });
            return Results.NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return Results.NotFound(ex);
        }
    }
}
