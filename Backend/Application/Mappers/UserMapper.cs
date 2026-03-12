using Alti.Domain.Entities;
using Application.Dtos.User;
using Application.DTOs.User;

namespace Application.Mappers;

public static class UserMapper
{
    public static UserResponseDto ToDto(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        FullName = $"{user.FirstName} {user.LastName}",
        Email = user.Email,
        Phone = user.Phone,
        Role = user.Role,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}