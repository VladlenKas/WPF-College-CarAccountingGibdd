using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int ApplicationId { get; set; }

    public string Number { get; set; } = null!;

    public DateOnly IssueDate { get; set; }

    public string LicensePlate { get; set; } = null!;

    public sbyte IsActive { get; set; }

    public virtual Application Application { get; set; } = null!;

    public string IsActiveName => IsActive switch
    {
        0 => "Действительный",
        1 => "Недействительный",
        _ => "NULL VALUE"
    };
}
