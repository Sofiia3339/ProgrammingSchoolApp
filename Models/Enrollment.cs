using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Enrollment
{
    public int Id { get; set; }

    public int Customerid { get; set; }

    public int Courseid { get; set; }

    public DateTime? Enrolledat { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Lessonprogress> Lessonprogresses { get; set; } = new List<Lessonprogress>();
}
