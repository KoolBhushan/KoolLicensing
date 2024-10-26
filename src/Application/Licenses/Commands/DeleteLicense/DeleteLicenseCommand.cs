using MediatR;

namespace KoolLicensing.Application.Licenses.Commands.DeleteLicense;
// Include properties to be used as input for the command
public record DeleteLicenseCommand(int ProductId, int LicenseId) : IRequest;
