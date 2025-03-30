using System;
using System.Collections.Generic;

namespace CarAccoutingGibdd.Model;

public partial class VehicleType
{
    public int VehicleTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
