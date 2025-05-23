﻿using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Violation
{
    public int ViolationsId { get; set; }

    public string Name { get; set; } = null!;

    public sbyte Deleted { get; set; }

    public virtual ICollection<ViolationsInspection> ViolationsInspections { get; set; } = new List<ViolationsInspection>();
}
