using System;
using System.Collections.Generic;

namespace ProgrammingSchoolApp.Models;

public partial class Coursepricehistory
{
    public int Id { get; set; }

    public int Courseid { get; set; }

    public int Adminid { get; set; }

    public decimal? Oldprice { get; set; }

    public decimal Newprice { get; set; }

    public DateTime? Changedat { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
