using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;

namespace CarAccoutingGibdd.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int PostId { get; set; }

    public int DepartmentId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Fullname => $"{Lastname} {Firstname} {Patronymic}";

    public string FIname => $"{Lastname} {Firstname}";
    
    public DateOnly Birthdate { get; set; }

    public string Passport { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public sbyte Deleted { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public virtual Post Post { get; set; } = null!;
}
