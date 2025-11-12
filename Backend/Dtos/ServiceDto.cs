using System.Text.Json.Serialization;
using Backend.Dtos;
using Backend.Models;
namespace Backend.Dtos
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        
    }
}