using KoolLicensing.Application.Common.Models;
using KoolLicensing.Application.Licenses.Commands.CreateLicense;
using MediatR;

namespace KoolLicensing.Application.Licenses.Queries.GetLicensesWithPagination;
// Include properties to be used as input for the query
public record GetLicensesWithPaginationQuery(int ProductId, int PageNumber, int PageSize) : IRequest<PaginatedList<LicenseResponse>>;
