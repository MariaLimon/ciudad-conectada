using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using ApiCC.Data;
using ApiCC.Models;

namespace Backend.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange (Preparación)
            var mockContext = new Mock<ApplicationDbContext>();
            var userToFind = new User { Id = 1, Name = "testuser", Email = "test@example.com" };

            // Creamos un mock del DbSet<User>
            var mockDbSet = new Mock<DbSet<User>>();
            
            // Configuramos el mock para que FindAsync devuelva nuestro usuario de prueba
            mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(userToFind);

            // Configuramos el mock del contexto para que devuelva nuestro mockDbSet
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var userService = new UserService(mockContext.Object);

            // Act (Acción)
            var result = await userService.GetUserByIdAsync(1);

            // Assert (Verificación)
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("testuser", result.Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<User>>();
            
            // Configuramos el mock para que FindAsync devuelva null
            mockDbSet.Setup(m => m.FindAsync(99)).ReturnsAsync((User?)null);

            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            var userService = new UserService(mockContext.Object);

            // Act
            var result = await userService.GetUserByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
    }
}