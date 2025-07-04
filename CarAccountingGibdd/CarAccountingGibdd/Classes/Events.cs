﻿using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes;

public class ApplicationEventArgs : EventArgs
{
    public Application Application { get; set; } = null!;
}

public class InspectionEventArgs : EventArgs
{
    public Inspection Inspection { get; set; } = null!;
}

public class ViolationEventArgs : EventArgs
{
    public Violation Violation { get; set; } = null!;
}

public class PhotosVehicleEventArgs : EventArgs
{
    public PhotosVehicle PhotosVehicle { get; set; } = null!;
}
