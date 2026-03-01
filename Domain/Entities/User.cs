using Alti.Core.Domain.Enums;
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

        public IReadOnlyCollection<Booking> GuestBookings
            => _guestBookings.AsReadOnly();

        private readonly List<Booking> _guestBookings = new();

        public string FullName => $"{FirstName} {LastName}";
        public bool IsAdministrator => Role == UserRole.Administrator;
        public bool IsReceptionist => Role == UserRole.Receptionist;
        public bool IsGuest => Role == UserRole.Guest;

        public static User Create(
            string firstName,
            string lastName,
            string email,
            string passwordHash,
            UserRole role = UserRole.Guest,
            string? phone = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));

            return new User
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email.Trim().ToLower(),
                PasswordHash = passwordHash,
                Phone = phone?.Trim(),
                Role = role
            };
        }

        public void UpdateDetails(string firstName, string lastName, string? phone)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Phone = phone?.Trim();
            RegisterUpdate();
        }

        public void ChangePassword(string newHash)
        {
            if (string.IsNullOrWhiteSpace(newHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(newHash));

            PasswordHash = newHash;
            RegisterUpdate();
        }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
            RegisterUpdate();
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException($"User {Id} is already inactive.");

            IsActive = false;
            RegisterUpdate();
        }

        public void Reactivate()
        {
            if (IsActive)
                throw new InvalidOperationException($"User {Id} is already active.");

            IsActive = true;
            RegisterUpdate();
        }

        public void VerifyRole(UserRole requiredRole)
        {
            if (Role != requiredRole)
                throw new AccessDeniedException(Id, requiredRole.ToString());
        }
    }
}
