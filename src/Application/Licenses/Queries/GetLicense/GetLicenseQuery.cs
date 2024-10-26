using KoolLicensing.Application.Licenses.Commands.CreateLicense;
using MediatR;

namespace KoolLicensing.Application.Licenses.Queries.GetLicense;
// Include properties to be used as input for the query
public record GetLicenseQuery(int ProductId, int LicenseId) : IRequest<LicenseResponse>;
