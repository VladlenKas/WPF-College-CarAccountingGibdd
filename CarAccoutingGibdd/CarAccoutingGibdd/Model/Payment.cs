using System;
using System.Collections.Generic;

namespace CarAccoutingGibdd.Model;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
