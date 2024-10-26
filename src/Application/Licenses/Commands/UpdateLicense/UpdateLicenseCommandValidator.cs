using FluentValidation;

namespace KoolLicensing.Application.Licenses.Commands.UpdateLicense;
public class UpdateLicenseCommandValidator : AbstractValidator<UpdateLicenseCommand>
{
    public UpdateLicenseCommandValidator()
    {
        RuleFor(x => x.LicenseId).NotEmpty().NotNull();
        RuleFor(x => x.ProductId).NotEmpty().NotNull();
        RuleFor(x => x.MaxNoOfMachines).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(x => x.ValidityInDays).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(x => x.LicenseType).NotEmpty().NotNull();
    }
}
