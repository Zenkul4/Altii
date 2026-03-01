using Alti.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class Room : BaseEntity
    {
        private Room() { }

        public string Number { get; private set; } = string.Empty;
        public RoomType Type { get; private set; }
        public short Floor { get; private set; }
        public short Capacity { get; private set; }
        public decimal BasePrice { get; private set; }
        public string? Description { get; private set; }
        public RoomStatus Status { get; private set; } = RoomStatus.Available;
        public int RowVersion { get; private set; } = 0;

        public IReadOnlyCollection<Booking> Bookings
            => _bookings.AsReadOnly();

        private readonly List<Booking> _bookings = new();

        public bool IsAvailable => Status == RoomStatus.Available;

        public static Room Create(
            string number,
            RoomType type,
            short floor,
            short capacity,
            decimal basePrice,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Room number cannot be empty.", nameof(number));

            if (basePrice <= 0)
                throw new ArgumentException("Base price must be greater than zero.", nameof(basePrice));

            if (capacity is < 1 or > 20)
                throw new ArgumentException("Capacity must be between 1 and 20.", nameof(capacity));

            if (floor < 0)
                throw new ArgumentException("Floor number cannot be negative.", nameof(floor));

            return new Room
            {
                Number = number.Trim().ToUpper(),
                Type = type,
                Floor = floor,
                Capacity = capacity,
                BasePrice = basePrice,
                Description = description?.Trim()
            };
        }

        public void Block()
        {
            if (!IsAvailable)
                throw new RoomNotAvailableException(Id, Status.ToString());

            Status = RoomStatus.Blocked;
            RegisterUpdate();
        }

        public void ReleaseBlock()
        {
            if (Status != RoomStatus.Blocked)
                throw new InvalidOperationException($"Room {Number} is not blocked. Current status: {Status}.");

            Status = RoomStatus.Available;
            RegisterUpdate();
        }

        public void MarkAsOccupied()
        {
            if (Status != RoomStatus.Blocked && Status != RoomStatus.Available)
                throw new RoomNotAvailableException(Id, Status.ToString());

            Status = RoomStatus.Occupied;
            RegisterUpdate();
        }

        public void SendToCleaning()
        {
            if (Status != RoomStatus.Occupied)
                throw new InvalidOperationException($"Only occupied rooms can be sent to cleaning. Current status: {Status}.");

            Status = RoomStatus.Cleaning;
            RegisterUpdate();
        }

        public void MarkAsAvailable()
        {
            if (Status == RoomStatus.Inactive)
                throw new InvalidOperationException($"Room {Number} is inactive and cannot be activated from this flow.");

            Status = RoomStatus.Available;
            RegisterUpdate();
        }

        public void Disable()
        {
            if (Status == RoomStatus.Occupied)
                throw new InvalidOperationException($"Room {Number} cannot be disabled while occupied.");

            Status = RoomStatus.Inactive;
            RegisterUpdate();
        }

        public void Enable()
        {
            if (Status != RoomStatus.Inactive)
                throw new InvalidOperationException($"Room {Number} is not inactive.");

            Status = RoomStatus.Available;
            RegisterUpdate();
        }

        public void UpdateDetails(decimal newBasePrice, string? newDescription)
        {
            if (newBasePrice <= 0)
                throw new ArgumentException("Base price must be greater than zero.", nameof(newBasePrice));

            BasePrice = newBasePrice;
            Description = newDescription?.Trim();
            RegisterUpdate();
        }
    }
}
