﻿using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public sbyte Deleted { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
