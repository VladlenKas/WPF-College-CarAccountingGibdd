using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public int VehicleTypeId { get; set; }

    public string Vin { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public short Year { get; set; }

    public string Color { get; set; } = null!;

    public string? LicensePlate { get; set; }

    public bool Used { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<PhotosVehicle> PhotosVehicles { get; set; } = new List<PhotosVehicle>();

    public virtual VehicleType VehicleType { get; set; } = null!;
}
