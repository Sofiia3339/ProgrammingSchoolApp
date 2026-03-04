using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Programminglanguage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
