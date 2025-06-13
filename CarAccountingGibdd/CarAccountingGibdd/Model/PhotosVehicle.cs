using CarAccountingGibdd.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace CarAccountingGibdd.Model;

public partial class PhotosVehicle
{
    public int PhotosVehicleId { get; set; }

    public int VehicleId { get; set; }

    public byte[] Photo { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;

    public BitmapImage BitmapImage => TypeHelper.GetBitmapImage(Photo);
}
