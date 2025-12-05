using Xunit;
using Moq;
using Backend.Models;
using Backend.Interfaces;
using Backend.Services;
using System.Threading.Tasks;

namespace Backend.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _repositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IRepository<User>>();
            _userService = new UserService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Name = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "hashedPassword",
                IsAdmin = false,
                Rol = "Usuario"
            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.Equal("Test", result.Name);
            Assert.Equal("User", result.LastName);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("Usuario", result.Rol);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateUserAsync_ShouldReturnUser_WhenPasswordIsCorrect()
        {
            // Arrange
            var plainPassword = "123456";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var user = new User
            {
                Id = 1,
                Name = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = hashedPassword,
                IsAdmin = false,
                Rol = "Usuario"
            };

            _repositoryMock
                .Setup(repo => repo.GetByConditionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidateUserAsync("test@example.com", plainPassword);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result!.Email);
        }

        [Fact]
        public async Task ValidateUserAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Name = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("correctPassword"),
                IsAdmin = false,
                Rol = "Usuario"
            };

            _repositoryMock
                .Setup(repo => repo.GetByConditionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidateUserAsync("test@example.com", "wrongPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(repo => repo.GetByConditionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.ValidateUserAsync("noexiste@test.com", "123456");

            // Assert
            Assert.Null(result);
        }
    }
}
