using System;
using System.Collections.Generic;
namespace ProjectManagement.Domain.Entities.Users
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;

        // Navigation property for users
        //public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
