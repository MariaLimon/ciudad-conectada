using Xunit;
using Microsoft.EntityFrameworkCore;
using ApiCC.Data;
using ApiCC.Models;

namespace Backend.Tests.Services
{
    public class UserServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Usamos la misma técnica de BD en memoria para simplicidad y robustez
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _userService = new UserService(_context);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange (Preparación)
            var userToFind = new User 
            { 
                Id = 1, 
                Name = "Test", 
                LastName = "User", 
                Email = "test@example.com",
                Password = "Password123", // En un caso real, esto sería un hash
                IsEmailConfirmed = true,
                IsAdmin = false
            };

            _context.Users.Add(userToFind);
            await _context.SaveChangesAsync();

            // Act (Acción)
            var result = await _userService.GetUserByIdAsync(1);

            // Assert (Verificación)
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name); // <-- Usamos la propiedad 'Name'
            Assert.Equal("User", result.LastName); // <-- Usamos la propiedad 'LastName'
            Assert.Equal("test@example.com", result.Email); // <-- Usamos la propiedad 'Email'
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _userService.GetUserByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
    }
}