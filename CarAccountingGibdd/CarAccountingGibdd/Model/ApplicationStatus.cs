using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class ApplicationStatus
{
    public int ApplicationStatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
