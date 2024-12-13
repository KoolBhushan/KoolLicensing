﻿// Author:
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

namespace KoolLicensing.Domain.Entities;
public class License : BaseAuditableEntity
{
    public LicenseKey? Key { get; set; }
    public LicenseType LicenseType { get; set; }
    public DateTimeOffset Expires { get; set; }
    public bool Feature0 { get; set; } = true;
    public bool Feature1 { get; set; } = false;
    public bool Feature2 { get; set; } = false;
    public bool Feature3 { get; set; } = false;
    public bool Feature4 { get; set; } = false;
    public bool Feature5 { get; set; } = false;
    public bool Feature6 { get; set; } = false;
    public bool Feature7 { get; set; } = false;
    public bool Feature8 { get; set; } = false;
    public bool Feature9 { get; set; } = false;
    public bool Block { get; set; } = false;
    public bool TrialActivation { get; set; } = false;
    public int MaxNoOfMachines { get; set; } = 1;
    public DateTimeOffset SignDate { get; set; }
    public string Signature { get; set; } = string.Empty;
    public string RawResponse { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public ICollection<Machine> ActivatedMachines { get; set; } = [];
    public Product Product { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
