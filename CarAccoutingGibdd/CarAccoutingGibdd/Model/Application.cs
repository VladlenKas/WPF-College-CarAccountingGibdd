using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;

namespace CarAccoutingGibdd.Model;

public partial class Application
{
    public int ApplicationId { get; set; }

    public int VehicleId { get; set; }

    public int ApplicationStatusId { get; set; }

    public DateTime DatetimeSupply { get; set; }

    public DateTime? DatetimeAccept { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
