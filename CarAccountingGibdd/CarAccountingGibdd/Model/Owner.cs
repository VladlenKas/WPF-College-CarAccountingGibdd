using System;
using System.Collections.Generic;

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

    public string Passport { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public sbyte Deleted { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<OwnerVehicle> OwnerVehicles { get; set; } = new List<OwnerVehicle>();
}
