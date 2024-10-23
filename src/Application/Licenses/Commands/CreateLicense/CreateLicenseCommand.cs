using KoolLicensing.Domain.Enums;
using MediatR;

namespace KoolLicensing.Application.Licenses.Commands.CreateLicense;
// Include properties to be used as input for the command
public record CreateLicenseCommand() : IRequest<LicenseResponse>
{
    public int ProductId {  get; set; }
    public int CustomerId { get; set; }
    public LicenseType LicenseType { get; set; }
    public int ValidityInDays { get; set; }
    public int MaxNoOfMachines { get; set; }
    public bool Feature1 { get; set; } = false;
    public bool Feature2 { get; set; } = false;
    public bool Feature3 { get; set; } = false;
    public bool Feature4 { get; set; } = false;
    public bool Feature5 { get; set; } = false;
    public bool Feature6 { get; set; } = false;
    public bool Feature7 { get; set; } = false;
    public bool Feature8 { get; set; } = false;
    public bool Feature9 { get; set; } = false;
}
