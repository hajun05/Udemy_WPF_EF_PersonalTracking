using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Udemy_WPF_EF_PersonalTracking.DB;

public partial class PersonaltrackingContext : DbContext
{
    public PersonaltrackingContext()
    {
    }

    public PersonaltrackingContext(DbContextOptions<PersonaltrackingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Permissionstate> Permissionstates { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Salarymonth> Salarymonths { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Taskstate> Taskstates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    {
        var coonLines = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\DB_Definition\Udemy_WPF_EF_PersonalTracking 연결 문자열.txt"));
        optionsBuilder.UseSqlServer(coonLines); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Department");

            entity.ToTable("DEPARTMENT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EMPLOYEE__3214EC270D0FB394");

            entity.ToTable("EMPLOYEE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.ImagePath).IsUnicode(false);
            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMPLOYEE_DEPARTMENT");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMPLOYEE_POSITION");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PERMISSI__3214EC27F380B49A");

            entity.ToTable("PERMISSION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.PermissionExplanation).IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERMISSION_EMPLOYEE");

            entity.HasOne(d => d.PermissionStateNavigation).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.PermissionState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERMISSION_PERMISSIONSTATE");
        });

        modelBuilder.Entity<Permissionstate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PERMISSI__3214EC2740287C89");

            entity.ToTable("PERMISSIONSTATE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__POSITION__3214EC2748F3BF8B");

            entity.ToTable("POSITION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Positions)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSITION_DEPARTMENT");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SALARY__3214EC2760CCC8D7");

            entity.ToTable("SALARY");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SALARY_EMPLOYEE");

            entity.HasOne(d => d.Month).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.MonthId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SALARY_SALARYMONTH");
        });

        modelBuilder.Entity<Salarymonth>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MONTHS__3214EC2794EC6130");

            entity.ToTable("SALARYMONTH");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MonthName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TASK__3214EC27C9FF229C");

            entity.ToTable("TASK");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.TaskContent).IsUnicode(false);
            entity.Property(e => e.TaskTitle)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TASK_EMPLOYEE");

            entity.HasOne(d => d.TaskStateNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TASK_TASKSTATE");
        });

        modelBuilder.Entity<Taskstate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TASKSTAT__3214EC277B16C4E7");

            entity.ToTable("TASKSTATE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
