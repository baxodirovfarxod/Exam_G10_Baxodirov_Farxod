﻿namespace UserContacts.Dal.Entities;

public class UserRole
{
    public long UserRoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }

    public List<User> Users { get; set; }
}
