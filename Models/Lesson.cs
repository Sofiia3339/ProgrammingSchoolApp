using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public int Courseid { get; set; }

    public string Title { get; set; } = null!;

    public string? Videourl { get; set; }

    public int Ordernumber { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lessonprogress> Lessonprogresses { get; set; } = new List<Lessonprogress>();
}
