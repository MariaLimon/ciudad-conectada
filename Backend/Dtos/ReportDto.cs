using System.Text.Json.Serialization;
using Backend.Dtos;
using Backend.Models;
namespace Backend.Dtos
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public UserDto? User { get; set; }
        public ServiceDto? Service { get; set; }
    }
}
