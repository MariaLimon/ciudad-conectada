using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string Company { get; set; } = string.Empty;

        public ICollection<Report>? ReportsAsService1 { get; set; }
        
    }
}