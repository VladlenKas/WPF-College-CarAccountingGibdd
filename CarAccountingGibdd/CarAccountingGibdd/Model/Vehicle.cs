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

    public string Info => $"{Brand} {Model} {Year}, {Color}";

    public string BrandModel => $"{Brand} {Model}";

    public sbyte Used { get; set; }

    public sbyte Deleted { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<PhotosVehicle> PhotosVehicles { get; set; } = new List<PhotosVehicle>();

    public virtual VehicleType VehicleType { get; set; } = null!;

    public string UsedValueString => Used switch
    {
        1 => "Нет",
        0 => "Да",
        _ => "NULL VALUE"
    };
}
