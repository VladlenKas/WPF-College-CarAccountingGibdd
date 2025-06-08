using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Violation
{
    public int ViolationId { get; set; }

    public string Description { get; set; } = null!;

    public string NumberDescription => $"(№{ViolationId}) {Description}";

    public sbyte Deleted { get; set; }

    public virtual ICollection<ViolationInspection> ViolationInspections { get; set; } = new List<ViolationInspection>();
}
