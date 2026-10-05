using Application.Dtos.User;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace Backend.Tests.Application;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    private static CreateUserDto CreateValidDto() => new()
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        Password = "Password123!",
        Phone = "+18095551234"
    };

    [Fact]
    public void Validate_WithValidDto_ShouldBeValid()
    {
        // Arrange
        var dto = CreateValidDto();

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenFirstNameIsEmptyOrNull_ShouldFailWithExpectedMessage(string? firstName)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.FirstName = firstName!;

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.FirstName) && e.ErrorMessage == "First name is required.");
    }

    [Fact]
    public void Validate_WhenFirstNameExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.FirstName = new string('A', 101);

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.FirstName));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenLastNameIsEmptyOrNull_ShouldFailWithExpectedMessage(string? lastName)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.LastName = lastName!;

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.LastName) && e.ErrorMessage == "Last name is required.");
    }

    [Fact]
    public void Validate_WhenLastNameExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.LastName = new string('B', 101);

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.LastName));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenEmailIsEmptyOrNull_ShouldFailWithExpectedMessage(string? email)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Email = email!;

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Email) && e.ErrorMessage == "Email is required.");
    }

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("@missingusername.com")]
    [InlineData("missingatsign.com")]
    [InlineData("test@@domain.com")]
    public void Validate_WhenEmailFormatIsInvalid_ShouldFailWithExpectedMessage(string invalidEmail)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Email = invalidEmail;

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Email) && e.ErrorMessage == "Invalid email format.");
    }

    [Fact]
    public void Validate_WhenEmailExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Email = new string('a', 250) + "@test.com";

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Email));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenPasswordIsEmptyOrNull_ShouldFailWithRequiredMessage(string? password)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Password = password!;

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Password) && e.ErrorMessage == "Password is required.");
    }

    [Theory]
    [InlineData("Pass1")]
    [InlineData("Ab1!")]
    public void Validate_WhenPasswordIsShorterThan8Characters_ShouldFailWithLengthMessage(string shortPassword)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Password = shortPassword;

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Password) && e.ErrorMessage == "Password must be at least 8 characters.");
    }

    [Fact]
    public void Validate_WhenPasswordHasNoUppercase_ShouldFailWithUppercaseMessage()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Password = "password123!";

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Password) && e.ErrorMessage == "Must contain at least one uppercase letter.");
    }

    [Fact]
    public void Validate_WhenPasswordHasNoNumber_ShouldFailWithNumberMessage()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Password = "PasswordWithoutNumber!";

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Password) && e.ErrorMessage == "Must contain at least one number.");
    }

    [Fact]
    public void Validate_WhenPhoneExceeds25Characters_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Phone = new string('1', 26);

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserDto.Phone));
    }

    [Fact]
    public void Validate_WhenPhoneIsNull_ShouldBeValid()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Phone = null;

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
