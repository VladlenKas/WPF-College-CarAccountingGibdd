using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Inspection
{
    public int InspectionId { get; set; }

    public int ApplicationId { get; set; }

    public int InspectorId { get; set; }

    public int StatusId { get; set; }

    public DateTime DatetimePlanned { get; set; }

    public DateTime? DatetimeCompleted { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual Employee Inspector { get; set; } = null!;

    public virtual InspectionStatus Status { get; set; } = null!;

    public virtual ICollection<ViolationsInspection> ViolationsInspections { get; set; } = new List<ViolationsInspection>();
}
