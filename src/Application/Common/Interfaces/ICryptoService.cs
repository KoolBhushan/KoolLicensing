using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoolLicensing.Domain.ValueObjects;

namespace KoolLicensing.Application.Common.Interfaces;
public interface ICryptoService
{
    string GenerateProductCode();

    LicenseKey GenerateKey();
}
