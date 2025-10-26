using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class InspectionStatus
{
    public int InspectionStatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
