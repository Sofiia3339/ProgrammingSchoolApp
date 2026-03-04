using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Coursehistory
{
    public int Historyid { get; set; }

    public int? Courseid { get; set; }

    public string? Changedescription { get; set; }

    public DateTime? Changedat { get; set; }
}
