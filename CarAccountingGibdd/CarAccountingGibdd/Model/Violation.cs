using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Violation
{
    public int ViolationId { get; set; }

    public int Number { get; set; }

    public string Description { get; set; } = null!;

    public string NumberDescription => $"(№{Number:D3}) {Description}";

    public sbyte Deleted { get; set; }

    public virtual ICollection<ViolationInspection> ViolationInspections { get; set; } = new List<ViolationInspection>();
}
