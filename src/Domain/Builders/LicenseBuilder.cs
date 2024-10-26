// Author:
// Bhushan Kamble
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoolLicensing.Domain.Entities;

namespace KoolLicensing.Domain.Builders;
public class LicenseBuilder : ILicenseBuilder
{
    private readonly License license;

    public LicenseBuilder()
    {
        this.license = new License();
        this.license.TrialActivation = false;
        license.Feature0 = true;
        license.Block = false;
        this.license.LicenseType = LicenseType.Subscription;
        this.license.Expires = DateTimeOffset.Now.AddDays(30);
        this.license.MaxNoOfMachines = 1;
    }

    public ILicenseBuilder AddValidDays(int days)
    {
        if (this.license.TrialActivation == false && this.license.LicenseType != LicenseType.Trial) 
        { 
            this.license.Expires = DateTimeOffset.UtcNow.AddDays(days);
        }

        return this;
    }

    public License Build()
    {
       return license;
    }

    public ILicenseBuilder ForCustomer(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException($"The argument {nameof(customer)} cannot be null.");
        }

        this.license.Customer = customer;
        this.license.CustomerId = customer.Id;
        return this;
    }

    public ILicenseBuilder ForProduct(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException($"The argument {nameof(product)} cannot be null.");
        }

        this.license.Product = product;
        this.license.ProductId = product.Id;
        return this;
    }

    public ILicenseBuilder OfType(LicenseType licenseType)
    {
        if(this.license.TrialActivation)
        {
            this.license.LicenseType = LicenseType.Trial;
            this.license.Expires = DateTimeOffset.UtcNow.AddDays(30);
        }
        else
        {
            this.license.LicenseType = licenseType;
        }

        return this;
    }

    public ILicenseBuilder SetAsTrial()
    {
        this.license.TrialActivation = true;
        this.license.LicenseType = LicenseType.Trial;
        this.license.Expires = DateTimeOffset.UtcNow.AddDays(30);
        return this;
    }

    public ILicenseBuilder WithFeature1(bool enableFeature1)
    {
       this.license.Feature1 = enableFeature1;
        return this;
    }

    public ILicenseBuilder WithFeature2(bool enableFeature2)
    {
        this.license.Feature2 = enableFeature2;
        return this;
    }

    public ILicenseBuilder WithFeature3(bool enableFeature3)
    {
        this.license.Feature3 = enableFeature3;
        return this;
    }

    public ILicenseBuilder WithFeature4(bool enableFeature4)
    {
        this.license.Feature4 = enableFeature4;
        return this;
    }

    public ILicenseBuilder WithFeature5(bool enableFeature5)
    {
        this.license.Feature5 = enableFeature5;
        return this;
    }

    public ILicenseBuilder WithFeature6(bool enableFeature6)
    {
        this.license.Feature6 = enableFeature6;
        return this;
    }

    public ILicenseBuilder WithFeature7(bool enableFeature7)
    {
        this.license.Feature7 = enableFeature7;
        return this;
    }

    public ILicenseBuilder WithFeature8(bool enableFeature8)
    {
        this.license.Feature8 = enableFeature8;
        return this;
    }

    public ILicenseBuilder WithFeature9(bool enableFeature9)
    {
        this.license.Feature9 = enableFeature9;
        return this;
    }

    public ILicenseBuilder WithKey(LicenseKey licenseKey)
    {
        if (licenseKey == null)
        {
            throw new ArgumentNullException($"The argument {nameof(licenseKey)} cannot be null.");
        }

        this.license.Key = licenseKey;
        return this;
    }

    public ILicenseBuilder WithMaxMachines(int maxMachines)
    {
        this.license.MaxNoOfMachines = maxMachines;
        return this;
    }
}
