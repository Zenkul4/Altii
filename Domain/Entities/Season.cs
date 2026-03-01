using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class Season : BaseEntity
    {
        private Season() { }

        public string Name { get; private set; } = string.Empty;
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal Multiplier { get; private set; } = 1.00m;
        public string? Description { get; private set; }
        public int CreatedById { get; private set; }

        public IReadOnlyCollection<Rate> Rates
            => _rates.AsReadOnly();

        private readonly List<Rate> _rates = new();

        public User CreatedBy { get; private set; } = null!;

        public int DurationInDays => EndDate.DayNumber - StartDate.DayNumber + 1;
        public bool ContainsDate(DateOnly date) => date >= StartDate && date <= EndDate;
        public bool OverlapsWith(DateOnly start, DateOnly end) => StartDate <= end && EndDate >= start;

        public static Season Create(
            string name,
            DateOnly startDate,
            DateOnly endDate,
            decimal multiplier,
            int createdById,
            string? description = null)
        {
            Validate(name, startDate, endDate, multiplier);

            return new Season
            {
                Name = name.Trim(),
                StartDate = startDate,
                EndDate = endDate,
                Multiplier = multiplier,
                CreatedById = createdById,
                Description = description?.Trim()
            };
        }

        public void Update(string name, DateOnly startDate, DateOnly endDate, decimal multiplier, string? description = null)
        {
            Validate(name, startDate, endDate, multiplier);

            Name = name.Trim();
            StartDate = startDate;
            EndDate = endDate;
            Multiplier = multiplier;
            Description = description?.Trim();
            RegisterUpdate();
        }

        private static void Validate(string name, DateOnly start, DateOnly end, decimal multiplier)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Season name cannot be empty.", nameof(name));

            if (end <= start)
                throw new ArgumentException($"End date ({end:dd/MM/yyyy}) must be after start date ({start:dd/MM/yyyy}).", nameof(end));

            if (multiplier <= 0)
                throw new ArgumentException("Multiplier must be greater than zero.", nameof(multiplier));
        }
    }
}
