using System;
using System.Collections.Generic;

namespace Udemy_WPF_EF_PersonalTracking.DB;

public partial class Employee
{
    public int Id { get; set; }

    public int UserNo { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int PositionId { get; set; }

    public int Salary { get; set; }

    public DateOnly? BirtyDay { get; set; } //EF Core(8 이상)와 .NET 6 이상 환경에서는 DATE 컬럼이 자동으로 DateOnly로 매핑되도록 지원
    // public DateTime? BirtyDay { get; set; }

    public string? Address { get; set; }

    public string? Password { get; set; }

    public bool? IsAdmin { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

}
