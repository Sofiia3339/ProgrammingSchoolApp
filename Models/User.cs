using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Customer? Customer { get; set; }
}
