using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Exceptions;
using Alti.Domain.Factories.Implementations;
using Alti.Domain.Services.Implementations;
using FluentAssertions;
using Xunit;

namespace Backend.Tests.Domain;

public class UserDomainServiceTests
{
    private readonly UserDomainService _domainService = new();
    private readonly UserFactory _factory = new();

    private User CreateTestUser(UserRole role = UserRole.Guest)
    {
        return _factory.Create("John", "Doe", "john.doe@example.com", "validHash", role);
    }

    [Fact]
    public void VerifyRole_WhenUserHasRequiredRole_ShouldNotThrow()
    {
        // Arrange
        var user = CreateTestUser(UserRole.Administrator);

        // Act
        var act = () => _domainService.VerifyRole(user, UserRole.Administrator);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void VerifyRole_WhenUserDoesNotHaveRequiredRole_ShouldThrowAccessDeniedException()
    {
        // Arrange
        var user = CreateTestUser(UserRole.Guest);

        // Act
        var act = () => _domainService.VerifyRole(user, UserRole.Administrator);

        // Assert
        act.Should().Throw<AccessDeniedException>()
            .WithMessage($"*User {user.Id} does not have the required role: Administrator*");
    }
}
