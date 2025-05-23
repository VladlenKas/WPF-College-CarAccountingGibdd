using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class OwnerVehicle
{
    public int OwnerVehicleId { get; set; }

    public int VehicleId { get; set; }

    public int OwnerId { get; set; }

    public sbyte Deleted { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
