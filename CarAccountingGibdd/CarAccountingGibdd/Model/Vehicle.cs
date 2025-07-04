﻿using System;
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

    public string Info => $"{Brand} {Model} {Year}";

    public string FullInfo => $"{Brand} {Model} {Year}, {Color}. VIN: {Vin}";

    public string ShortInfo => $"{Brand} {Model}. {Vin}";

    public string BrandModel => $"{Brand} {Model}";

    public sbyte Used { get; set; }

    public sbyte Deleted { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<PhotosVehicle> PhotosVehicles { get; set; } = new List<PhotosVehicle>();

    public virtual VehicleType VehicleType { get; set; } = null!;

    // Владелец
    public Owner? Owner => Applications?
        .SingleOrDefault(a => a.Certificates?
        .Any(c => c.IsActive == 1) == true)?
        .Owner ?? null;

    // Фи и пасспорт владельца
    public string OwnerPassport =>
        Owner?.Passport ?? "Владелец отсутствует";

    public string UsedValueString => Used switch
    {
        0 => "Нет",
        1 => "Да",
        _ => "NULL VALUE"
    };

    public string LicensePlateValue =>
        !string.IsNullOrEmpty(LicensePlate) ? LicensePlate : "Отсутствует";
}
