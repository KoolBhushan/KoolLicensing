using FluentValidation;

namespace KoolLicensing.Application.Licenses.Commands.CreateLicense;
public class CreateLicenseCommandValidator : AbstractValidator<CreateLicenseCommand>
{
    public CreateLicenseCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().NotNull();
        RuleFor(x => x.ProductId).NotEmpty().NotNull();
        RuleFor(x => x.MaxNoOfMachines).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(x => x.ValidityInDays).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(x => x.LicenseType).NotEmpty().NotNull();
    }
}
