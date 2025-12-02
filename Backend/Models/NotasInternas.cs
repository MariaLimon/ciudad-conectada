using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class NotasInternas
    {
        public int Id { get; set; }
        [Required]
        public int ReportId { get; set; } 
        [ForeignKey("ReportId")]
        public Report? Report { get; set; }
        // Usuario que hizo el reporte
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}