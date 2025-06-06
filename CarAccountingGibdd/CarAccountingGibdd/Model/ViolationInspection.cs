using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class ViolationInspection
{
    public int ViolationInspectionId { get; set; }

    public int ViolationId { get; set; }

    public int InspectionId { get; set; }

    public virtual Inspection Inspection { get; set; } = null!;

    public virtual Violation Violation { get; set; } = null!;
}
