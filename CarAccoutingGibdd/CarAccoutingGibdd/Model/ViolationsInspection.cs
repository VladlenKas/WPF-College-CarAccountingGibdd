using System;
using System.Collections.Generic;

namespace CarAccoutingGibdd.Model;

public partial class ViolationsInspection
{
    public int ViolationsInspectionId { get; set; }

    public int ViolationsId { get; set; }

    public int InspectionId { get; set; }

    public virtual Inspection Inspection { get; set; } = null!;

    public virtual Violation Violations { get; set; } = null!;
}
