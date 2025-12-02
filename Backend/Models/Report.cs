using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Report
    {
        public int Id { get; set; }

        // Usuario que hizo el reporte
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Servicio 1
        [Required]
        public int ServiceId1 { get; set; }
        [ForeignKey("ServiceId1")]
        public Service? Service1 { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ICollection<NotasInternas>? NotasInternas { get; set; } = new List<NotasInternas>();

        [Required]
        public string Estado { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public List<string>? Evidencias { get; set; } = new();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}