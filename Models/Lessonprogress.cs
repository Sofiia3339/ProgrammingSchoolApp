using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Lessonprogress
{
    public int Id { get; set; }

    public int Enrollmentid { get; set; }

    public int Lessonid { get; set; }

    public bool? Iscompleted { get; set; }

    public DateTime? Completedat { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;

    public virtual Lesson Lesson { get; set; } = null!;
}
