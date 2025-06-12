using System;
using System.Collections.Generic;

namespace Udemy_WPF_EF_PersonalTracking.DB;

public partial class Taskstate
{
    public int Id { get; set; }

    public string StateName { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
