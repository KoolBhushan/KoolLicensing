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

using System.Security.Cryptography;
using System.Text;
using KoolLicensing.Application.Common.Interfaces;
using KoolLicensing.Domain.ValueObjects;

namespace KoolLicensing.Application.Common.Security;
public class CryptoService : ICryptoService
{
    public string GenerateProductCode()
    {
        var stringLength = 8;

        // Generate a new GUID
        string guid = Guid.NewGuid().ToString("N");

        // Take the specified number of characters from the beginning of the string
        var result = guid.Substring(0, stringLength);

        return result.ToUpperInvariant();
    }

    public LicenseKey GenerateKey()
    {
        // Generate a new GUID
        string guid = Guid.NewGuid().ToString("D");

        var split = guid.Split('-');

        var keyBuilder = new StringBuilder();
        foreach (var key in split) 
        {
            keyBuilder.Append(key.Substring(0, 4));
            keyBuilder.Append('-');
        }

        var value = keyBuilder.ToString().TrimEnd('-').ToUpperInvariant();
        var hash = string.Empty;

        // Initialize a SHA256 hash object
        using (SHA256 sha256 = SHA256.Create())
        {
            // Compute the hash of the given string
            byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));

            // Convert the byte array to string format
            foreach (byte b in hashValue)
            {
                hash += $"{b:X2}";
            }
        }

       return new LicenseKey { Value = value, Hash = hash };
    }
}
