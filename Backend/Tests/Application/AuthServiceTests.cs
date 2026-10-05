using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Exceptions;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Alti.Domain.Interfaces.Repositories;
using Alti.Domain.Services.Interfaces;
using Application.Dtos.Auth;
using Application.Dtos.User;
using Application.Interfaces;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Application;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow = new();
    private readonly Mock<IUserRepository> _mockUserRepo = new();
    private readonly Mock<IUserFactory> _mockFactory = new();
    private readonly Mock<IUserDomainService> _mockDomainService = new();
    private readonly Mock<IPasswordHasher> _mockPasswordHasher = new();
    private readonly Mock<IJwtService> _mockJwt = new();

    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _mockUow.Setup(u => u.Users).Returns(_mockUserRepo.Object);

        _sut = new AuthService(
            _mockUow.Object,
            _mockFactory.Object,
            _mockDomainService.Object,
            _mockPasswordHasher.Object,
            _mockJwt.Object);
    }

    private static User CreateUser(string email, string passwordHash, bool isActive = true, UserRole role = UserRole.Guest)
    {
        var factory = new Alti.Domain.Factories.Implementations.UserFactory();
        var user = factory.Create("John", "Doe", email, passwordHash, role);
        if (!isActive)
        {
            var ds = new Alti.Domain.Services.Implementations.UserDomainService();
            ds.Deactivate(user);
        }
        return user;
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentialsAndActiveUser_ShouldReturnLoginResponse()
    {
        // Arrange
        var email = "john.doe@example.com";
        var password = "ValidPassword123";
        var passwordHash = "hashedPassword";
        var user = CreateUser(email, passwordHash, isActive: true, role: UserRole.Guest);
        var expectedToken = "jwt.test.token";
        var expectedExpiration = DateTime.UtcNow.AddHours(2);

        _mockUserRepo.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher.Setup(p => p.Verify(password, passwordHash))
            .Returns(true);
        _mockJwt.Setup(j => j.GenerateToken(user))
            .Returns(expectedToken);
        _mockJwt.Setup(j => j.GetExpiration())
            .Returns(expectedExpiration);

        var loginDto = new LoginDto { Email = email, Password = password };

        // Act
        var result = await _sut.LoginAsync(loginDto);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(email);
        result.FullName.Should().Be("John Doe");
        result.Role.Should().Be(UserRole.Guest);
        result.IsActive.Should().BeTrue();
        result.Token.Should().Be(expectedToken);
        result.ExpiresAt.Should().Be(expectedExpiration);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        _mockUserRepo.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var loginDto = new LoginDto { Email = email, Password = "AnyPassword123" };

        // Act
        var act = () => _sut.LoginAsync(loginDto);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Credenciales incorrectas.");
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsIncorrect_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var email = "john.doe@example.com";
        var password = "WrongPassword";
        var user = CreateUser(email, "hashedPassword");

        _mockUserRepo.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher.Setup(p => p.Verify(password, user.PasswordHash))
            .Returns(false);

        var loginDto = new LoginDto { Email = email, Password = password };

        // Act
        var act = () => _sut.LoginAsync(loginDto);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Credenciales incorrectas.");
    }

    [Fact]
    public async Task LoginAsync_WhenUserIsInactive_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var email = "inactive@example.com";
        var password = "ValidPassword123";
        var user = CreateUser(email, "hashedPassword", isActive: false);

        _mockUserRepo.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswordHasher.Setup(p => p.Verify(password, user.PasswordHash))
            .Returns(true);

        var loginDto = new LoginDto { Email = email, Password = password };

        // Act
        var act = () => _sut.LoginAsync(loginDto);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Credenciales incorrectas.");
    }

    [Fact]
    public async Task RegisterAsync_WithNewEmail_ShouldHashPasswordCreateUserAndSave()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "Alice",
            LastName = "Wonderland",
            Email = "alice@example.com",
            Password = "Password123",
            Phone = "+18091234567"
        };

        var hashedPassword = "securelyHashedPassword";
        var createdUser = CreateUser(dto.Email, hashedPassword, isActive: true, role: UserRole.Guest);

        _mockUserRepo.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockPasswordHasher.Setup(p => p.Hash(dto.Password))
            .Returns(hashedPassword);
        _mockFactory.Setup(f => f.Create(dto.FirstName, dto.LastName, dto.Email, hashedPassword, UserRole.Guest, dto.Phone))
            .Returns(createdUser);

        // Act
        var result = await _sut.RegisterAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(dto.Email);
        result.FirstName.Should().Be("John"); // from createdUser
        result.Role.Should().Be(UserRole.Guest);

        _mockUserRepo.Verify(r => r.AddAsync(createdUser, It.IsAny<CancellationToken>()), Times.Once);
        _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "Alice",
            LastName = "Wonderland",
            Email = "existing@example.com",
            Password = "Password123"
        };

        _mockUserRepo.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = () => _sut.RegisterAsync(dto);

        // Assert
        await act.Should().ThrowAsync<DuplicateEmailException>()
            .WithMessage($"*Email '{dto.Email}' is already registered*");

        _mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
