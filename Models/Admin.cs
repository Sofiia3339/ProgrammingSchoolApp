using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Admin
{
    public int Userid { get; set; }

    public string Employeecode { get; set; } = null!;

    public string? Position { get; set; }

    public virtual ICollection<Coursepricehistory> Coursepricehistories { get; set; } = new List<Coursepricehistory>();

    public virtual User User { get; set; } = null!;
}
