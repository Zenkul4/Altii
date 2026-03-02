using Alti.Domain.Enums;
using Alti.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class User : BaseEntity
    {
        private User() { }

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string? Phone { get; private set; }
        public UserRole Role { get; private set; } = UserRole.Guest;
        public bool IsActive { get; private set; } = true;

    }
}
