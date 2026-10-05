using Alti.Domain.Enums;
using Alti.Domain.Factories.Implementations;
using FluentAssertions;
using Xunit;

namespace Backend.Tests.Domain;

public class UserFactoryTests
{
    private readonly UserFactory _factory = new();

    [Fact]
    public void Create_WithValidParameters_ShouldReturnUserWithExpectedProperties()
    {
        // Arrange
        var firstName = "  John  ";
        var lastName = "  Doe  ";
        var email = "  John.Doe@EXAMPLE.COM  ";
        var passwordHash = "hashedPassword123";
        var role = UserRole.Administrator;
        var phone = "  +1-809-555-1234  ";

        // Act
        var user = _factory.Create(firstName, lastName, email, passwordHash, role, phone);

        // Assert
        user.Should().NotBeNull();
        user.FirstName.Should().Be("John");
        user.LastName.Should().Be("Doe");
        user.Email.Should().Be("john.doe@example.com");
        user.PasswordHash.Should().Be("hashedPassword123");
        user.Role.Should().Be(UserRole.Administrator);
        user.Phone.Should().Be("+1-809-555-1234");
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithoutRole_ShouldDefaultToGuest()
    {
        // Act
        var user = _factory.Create("Jane", "Smith", "jane@example.com", "hash123");

        // Assert
        user.Role.Should().Be(UserRole.Guest);
        user.Phone.Should().BeNull();
        user.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidFirstName_ShouldThrowArgumentException(string? invalidFirstName)
    {
        // Act
        var act = () => _factory.Create(invalidFirstName!, "Doe", "test@test.com", "hash");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*First name cannot be empty*")
            .WithParameterName("firstName");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidLastName_ShouldThrowArgumentException(string? invalidLastName)
    {
        // Act
        var act = () => _factory.Create("John", invalidLastName!, "test@test.com", "hash");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Last name cannot be empty*")
            .WithParameterName("lastName");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException(string? invalidEmail)
    {
        // Act
        var act = () => _factory.Create("John", "Doe", invalidEmail!, "hash");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email cannot be empty*")
            .WithParameterName("email");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidPasswordHash_ShouldThrowArgumentException(string? invalidHash)
    {
        // Act
        var act = () => _factory.Create("John", "Doe", "test@test.com", invalidHash!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Password hash cannot be empty*")
            .WithParameterName("passwordHash");
    }
}
