using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? Levelid { get; set; }

    public int? Languageid { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Coursepricehistory> Coursepricehistories { get; set; } = new List<Coursepricehistory>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Programminglanguage? Language { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual Courselevel? Level { get; set; }
}
