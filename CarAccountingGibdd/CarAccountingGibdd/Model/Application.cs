using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Application
{
    public int ApplicationId { get; set; }

    public int ApplicationStatusId { get; set; }

    public int? OwnerVehicleId { get; set; }

    public DateTime DatetimeSupply { get; set; }

    public DateTime? DatetimeAccept { get; set; }

    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public virtual OwnerVehicle OwnerVehicle { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
