using KoolLicensing.Domain.Enums;

namespace KoolLicensing.Application.Licenses.Commands.CreateLicense;

public record LicenseResponse
{
    public string Key { get; set; } = string.Empty;

    public LicenseType LicenseType {  get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public DateTimeOffset Expires { get; set; }
}
