using System;
using System.Collections.Generic;

namespace PDAB.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public int EmployeeId { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
