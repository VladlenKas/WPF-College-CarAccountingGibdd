using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Post
{
    public int PostId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
