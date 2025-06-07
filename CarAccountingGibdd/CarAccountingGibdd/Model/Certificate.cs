using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int ApplicationId { get; set; }

    public string Number { get; set; } = null!;

    public DateOnly IssueDate { get; set; }

    public sbyte IsActive { get; set; }

    public virtual Application Application { get; set; } = null!;

    public string IsActiveName => IsActive == 0 ? "Действительный" : "Недействительный";
}
