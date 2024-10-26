using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoolLicensing.Domain.Builders;
public interface ILicenseBuilder
{
    public ILicenseBuilder ForProduct(Product product);

    public ILicenseBuilder ForCustomer(Customer customer);

    public ILicenseBuilder WithKey(LicenseKey licenseKey);

    public ILicenseBuilder OfType(LicenseType licenseType);

    public ILicenseBuilder AddValidDays(int days);

    public ILicenseBuilder WithMaxMachines(int maxMachines);

    public ILicenseBuilder SetAsTrial();

    public ILicenseBuilder WithFeature1(bool enableFeature1);

    public ILicenseBuilder WithFeature2(bool enableFeature2);

    public ILicenseBuilder WithFeature3(bool enableFeature3);

    public ILicenseBuilder WithFeature4(bool enableFeature4);

    public ILicenseBuilder WithFeature5(bool enableFeature5);

    public ILicenseBuilder WithFeature6(bool enableFeature6);

    public ILicenseBuilder WithFeature7(bool enableFeature7);

    public ILicenseBuilder WithFeature8(bool enableFeature8);

    public ILicenseBuilder WithFeature9(bool enableFeature9);

    public License Build();

}
