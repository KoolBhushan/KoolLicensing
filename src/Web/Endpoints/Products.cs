
using KoolLicensing.Application.Common.Models;
using KoolLicensing.Application.Products.Commands;
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
            .MapPut(UpdateProductAsync, "{id}")
            .MapPost(CreateProductAsync);
    }
    public async Task<IResult> GetProductAsync(ISender sender, int id)
    {
        var resposne = await sender.Send(new GetProductQuery { Id = id });
        return Results.Ok(resposne);
    }

    public async Task<PaginatedList<ProductResponse>> GetProductsWithPaginationAsync(ISender sender, [AsParameters] GetProductsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<int> CreateProductAsync(ISender sender, CreateProductCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<IResult> UpdateProductAsync(ISender sender, int id, UpdateProductCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
}
