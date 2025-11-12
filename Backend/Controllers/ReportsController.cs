using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services; 
using Backend.Dtos;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportsController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] Report report)
    {
        if (report == null)
        {
            return BadRequest("El reporte es inválido.");
        }

        var nuevoReporte = await _reporteService.CrearReporteAsync(report);

        return CreatedAtAction(nameof(GetReportById), new { id = nuevoReporte.Id }, nuevoReporte);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ReportDto>> GetReportById(int id)
    {
        var reportEntity = await _reporteService.ObtenerReportePorIdAsync(id);

        if (reportEntity == null)
        {
            return NotFound();
        }

        var reportDto = new ReportDto
        {
            Id = reportEntity.Id,
            Title = reportEntity.Title,
            Description = reportEntity.Description,
            Estado = reportEntity.Estado,
            Location = reportEntity.Location,
            CreatedAt = reportEntity.CreatedAt,
            User = reportEntity.User == null ? null : new UserDto
            {
                Id = reportEntity.User.Id,
                Name = reportEntity.User.Name,
                LastName = reportEntity.User.LastName,
                Email = reportEntity.User.Email
            },
            Service = reportEntity.Service == null ? null : new ServiceDto
            {
                Id = reportEntity.Service.Id,
                Type = reportEntity.Service.Type,
                Company = reportEntity.Service.Company
            }
        };

        return Ok(reportDto);
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> GetReportsByUserId(int usuarioId)
    {
        var reportes = await _reporteService.ObtenerReportesDeUsuarioAsync(usuarioId);
        return Ok(reportes);
    }
}