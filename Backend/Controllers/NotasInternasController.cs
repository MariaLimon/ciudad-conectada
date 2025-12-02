using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Backend.Data;
using Backend.Models;
using Backend.Dtos;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/reports/{reportId}/notas")]
    [Authorize] 
    public class NotasInternasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotasInternasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotaInterna(int reportId, [FromBody] NotaInternaCreateDto dto)
        {
            var reporte = await _context.Reports.FindAsync(reportId);
            if (reporte == null)
                return NotFound("El reporte no existe.");

            var nuevaNota = new NotasInternas
            {
                ReportId = reportId,
                UserId = dto.UserId,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.NotasInternas.Add(nuevaNota);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotasPorReporte), new { reportId }, nuevaNota);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotasInternas>>> GetNotasPorReporte(int reportId)
        {
            var notas = await _context.NotasInternas
                .Where(n => n.ReportId == reportId)
                .Include(n => n.User) 
                .ToListAsync();

            if (!notas.Any())
                return NotFound("Este reporte no tiene notas internas.");

            return Ok(notas);
        }
    }
}
