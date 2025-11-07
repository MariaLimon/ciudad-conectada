using Xunit;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ApiCC.Models;
using System.Text.Json;

namespace Backend.Tests.Controllers
{
    public class UsersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsers_ReturnsSuccessAndCorrectContentType()
        {
            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetUsers_ReturnsListOfUsers()
        {
            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            response.EnsureSuccessStatusCode();
            
            var stringResponse = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(stringResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(users);
            Assert.IsType<List<User>>(users);
        }
    }
}