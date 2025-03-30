using System;
using System.Collections.Generic;

namespace CarAccoutingGibdd.Model;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public int OwnerId { get; set; }

    public int VehicleTypeId { get; set; }

    public string Vin { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public short Year { get; set; }

    public string Color { get; set; } = null!;

    public string? LicensePlate { get; set; }

    public sbyte Used { get; set; }

    public sbyte Deleted { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<PhotosVehicle> PhotosVehicles { get; set; } = new List<PhotosVehicle>();

    public virtual VehicleType VehicleType { get; set; } = null!;
}
