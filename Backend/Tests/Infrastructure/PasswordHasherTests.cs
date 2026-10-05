using FluentAssertions;
using Infrastructure.Security.Implementations;
using Xunit;

namespace Backend.Tests.Infrastructure;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void Hash_WithValidPassword_ShouldReturnValidBcryptHash()
    {
        // Arrange
        var password = "MySecurePassword123!";

        // Act
        var hash = _hasher.Hash(password);

        // Assert
        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().StartWith("$2"); // BCrypt prefix
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Hash_WithEmptyOrNullPassword_ShouldThrowArgumentException(string? invalidPassword)
    {
        // Act
        var act = () => _hasher.Hash(invalidPassword!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Password cannot be empty*")
            .WithParameterName("password");
    }

    [Fact]
    public void Verify_WithCorrectPasswordAndHash_ShouldReturnTrue()
    {
        // Arrange
        var password = "CorrectPassword123";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Verify_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "CorrectPassword123";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify("WrongPassword", hash);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Verify_WithEmptyPassword_ShouldThrowArgumentException(string? invalidPassword)
    {
        // Arrange
        var hash = _hasher.Hash("Test1234");

        // Act
        var act = () => _hasher.Verify(invalidPassword!, hash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Password cannot be empty*")
            .WithParameterName("password");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Verify_WithEmptyHash_ShouldThrowArgumentException(string? invalidHash)
    {
        // Act
        var act = () => _hasher.Verify("Password123", invalidHash!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Hash cannot be empty*")
            .WithParameterName("hash");
    }
}
