
using KoolLicensing.Application.Common.Exceptions;
using KoolLicensing.Application.Common.Models;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;
using KoolLicensing.Application.Licenses.Commands.DeleteLicense;
using KoolLicensing.Application.Licenses.Commands.UpdateLicense;
using KoolLicensing.Application.Licenses.Queries.GetLicense;
using KoolLicensing.Application.Licenses.Queries.GetLicensesWithPagination;
using KoolLicensing.Application.Products.Commands;
using KoolLicensing.Application.Products.Queries;
using KoolLicensing.Application.Products.Queries.GetProduct;
using KoolLicensing.Application.Products.Queries.GetProductsWithPagination;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            .MapPost(CreateProductAsync)
            .MapGet(GetLicenseAsync, "{id}/licenses/{licId}")
            .MapDelete(DeleteLicenseAsync, "{id}/licenses/{licId}")
            .MapPut(UpdateLicenseAsync, "{id}/licenses/{licId}")
            .MapGet(GetLicensesWithPaginationAsync, "{id}/licenses")
            .MapPost(CreateLicenseAsync, "{id}/licenses");
    }

    #region Products
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

    #endregion

    #region Licenses
    public async Task<IResult> CreateLicenseAsync(ISender sender, [FromRoute] int id, [FromBody] CreateLicenseCommand command)
    {
        if (id != command.ProductId) return Results.BadRequest();

        var response = await sender.Send(command);
        return Results.Created();
    }

    public async Task<IResult> UpdateLicenseAsync(ISender sender, [FromRoute] int id, [FromRoute] int licId, [FromBody] UpdateLicenseCommand command)
    {
        if (id != command.ProductId) return Results.BadRequest();

        if (licId != command.LicenseId) return Results.BadRequest();

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

    public async Task<IResult> DeleteLicenseAsync(ISender sender, [FromRoute] int id, [FromRoute] int licId)
    {
        try
        {
            await sender.Send(new DeleteLicenseCommand(id, licId));
            return Results.NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return Results.NotFound(ex);
        }
    }

    public async Task<IResult> GetLicenseAsync(ISender sender, [FromRoute] int id, [FromRoute] int licId)
    {
        try
        {
            var resposne = await sender.Send(new GetLicenseQuery(id, licId));
            return Results.Ok(resposne);
        }
        catch (EntityNotFoundException ex)
        {
            return Results.NotFound(ex);
        }
    }

    public async Task<PaginatedList<LicenseResponse>> GetLicensesWithPaginationAsync(ISender sender, [FromRoute] int id, [FromQuery] int pageNo, [FromQuery] int pageSize)
    {
        return await sender.Send(new GetLicensesWithPaginationQuery(id, pageNo, pageSize));
    }

    #endregion
}
