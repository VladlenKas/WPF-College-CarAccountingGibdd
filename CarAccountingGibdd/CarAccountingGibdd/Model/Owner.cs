using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text;

namespace CarAccountingGibdd.Model;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Fullname => $"{Lastname} {Firstname} {Patronymic}";

    public string FIname => $"{Lastname} {Firstname}";

    public string FIpassport => $"{Lastname} {Firstname}, {Passport}";

    public DateOnly Birthdate { get; set; }

    public string? Email { get; set; } 

    public string Passport { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public sbyte Deleted { get; set; }

    public int VehiclesCount
    {
        get
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            Applications?
                .Where(a => a.Certificates?.Any(c => c.IsActive == 0) == true)?
                .ToList()
                .ForEach(a =>
                {
                    vehicles.Add(a.Vehicle);
                });
            return vehicles.Count;
        }
    }

    public string VehiclesNames
    {
        get
        {
            var vehiclesList = new StringBuilder();
            int counter = 1;

            // Находим все автомобили, которые связаны с текущим владельцем
            Applications?
                .Where(a => a.Certificates?.Any(c => c.IsActive == 0) == true)?
                .ToList()
                .ForEach(a =>
                {
                    vehiclesList.AppendLine($"{counter++}. {a.Vehicle.FullInfo}");
                });

            if (vehiclesList.Length > 0)
            {
                return vehiclesList.ToString();
            }

            return "У владельца нет транспортных средств";
        }
    }

    public string EmailValue =>
        !string.IsNullOrEmpty(Email) ? Email : "Отсутствует";

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
