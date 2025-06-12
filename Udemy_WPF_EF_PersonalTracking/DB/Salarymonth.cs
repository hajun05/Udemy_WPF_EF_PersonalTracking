using System;
using System.Collections.Generic;

namespace Udemy_WPF_EF_PersonalTracking.DB;

public partial class Salarymonth
{
    public int Id { get; set; }

    public string MonthName { get; set; } = null!;

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
