using System;
using System.Collections.Generic;

namespace CarAccoutingGibdd.Model;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int ApplicationId { get; set; }

    public string Number { get; set; } = null!;

    public string LicensePlate { get; set; } = null!;

    public DateOnly IssueDate { get; set; }

    public virtual Application Application { get; set; } = null!;
}
