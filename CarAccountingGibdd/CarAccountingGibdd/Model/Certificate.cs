using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int OwnerVehicleId { get; set; }

    public string Number { get; set; } = null!;

    public string LicensePlate { get; set; } = null!;

    public sbyte IsActive { get; set; }

    public virtual OwnerVehicle OwnerVehicle { get; set; } = null!;
}
