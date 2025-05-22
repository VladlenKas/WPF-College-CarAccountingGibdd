using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly Birthdate { get; set; }

    public string Passport { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public sbyte Deleted { get; set; }

    public virtual ICollection<OwnerVehicle> OwnerVehicles { get; set; } = new List<OwnerVehicle>();
}
