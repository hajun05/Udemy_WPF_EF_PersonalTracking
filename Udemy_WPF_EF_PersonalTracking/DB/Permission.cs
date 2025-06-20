﻿using System;
using System.Collections.Generic;

namespace Udemy_WPF_EF_PersonalTracking.DB;

public partial class Permission
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly PermissionStartDate { get; set; }

    public DateOnly PermissionEndDate { get; set; }

    public int PermissionState { get; set; }

    public string? PermissionExplanation { get; set; }

    public int PermissionDay { get; set; }

    public int UserNo { get; set; }

    public int PermissionAmount { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Permissionstate PermissionStateNavigation { get; set; } = null!;
}
