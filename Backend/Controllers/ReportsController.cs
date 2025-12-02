using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;
using Backend.Data;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReporteService _reporteService;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private static readonly string[] EstadosValidos = { "Enviado", "En Proceso", "Resuelto" };

    public ReportsController(IReporteService reporteService, ApplicationDbContext context, IWebHostEnvironment env)
    {
        _reporteService = reporteService;
        _context = context;
        _env = env;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportDto createReportDto)
    {
        if (createReportDto == null)
        {
            return BadRequest("El reporte es inválido.");
        }

        // 1. Mapea el DTO a una nueva entidad Report
        var nuevoReporteEntity = new Report
        {
            UserId = createReportDto.UserId,
            ServiceId1 = createReportDto.ServiceId1,
            Title = createReportDto.Title,
            Description = createReportDto.Description,
            Estado = createReportDto.Estado,
            Location = createReportDto.Location,
            CreatedAt = DateTime.UtcNow // ¡Importante! Establece la fecha de creación aquí.
        };

        // 2. Pasa la entidad completa al servicio
        var reporteCreado = await _reporteService.CrearReporteAsync(nuevoReporteEntity);

        // 3. Devuelve una respuesta con el DTO del reporte creado (opcional, pero es buena práctica)
        var reporteDto = new ReportDto
        {
            Id = reporteCreado.Id,
            Title = reporteCreado.Title,
            Description = reporteCreado.Description,
            Estado = reporteCreado.Estado,
            Location = reporteCreado.Location,
            CreatedAt = reporteCreado.CreatedAt,
            User = reporteCreado.User == null ? null : new UserDto
            {
                Id = reporteCreado.User.Id,
                Name = reporteCreado.User.Name,
                LastName = reporteCreado.User.LastName,
                Email = reporteCreado.User.Email
            },
            Service = reporteCreado.Service1 == null ? null : new ServiceDto
            {
                Id = reporteCreado.Service1.Id,
                Type = reporteCreado.Service1.Type,
                Company = reporteCreado.Service1.Company
            }
        };

        return CreatedAtAction(nameof(GetReportById), new { id = reporteCreado.Id }, reporteDto);
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
            Service = reportEntity.Service1 == null ? null : new ServiceDto
            {
                Id = reportEntity.Service1.Id,
                Type = reportEntity.Service1.Type,
                Company = reportEntity.Service1.Company
            }

        };

        return Ok(reportDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReportDto>>> GetReports()
    {
        var reports = await _reporteService.GetReportsAsync();

        var reportDtos = reports.Select(reportEntity => new ReportDto
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
            Service = reportEntity.Service1 == null ? null : new ServiceDto
            {
                Id = reportEntity.Service1.Id,
                Type = reportEntity.Service1.Type,
                Company = reportEntity.Service1.Company
            }
        }).ToList();

        return Ok(reportDtos);
    }



[HttpGet("usuario/{usuarioId}")]
public async Task<IActionResult> GetReportsByUserId(int usuarioId)
{
    // Obtén las entidades de la base de datos
    var reportesEntities = await _reporteService.ObtenerReportesDeUsuarioAsync(usuarioId);

    if (reportesEntities == null || !reportesEntities.Any())
    {
        return Ok(new List<ReportDto>()); // Devuelve una lista vacía si no hay reportes
    }

    // Proyecta las entidades a DTOs para evitar la referencia circular
    var reportDtos = reportesEntities.Select(reportEntity => new ReportDto
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
        Service = reportEntity.Service1 == null ? null : new ServiceDto
        {
            Id = reportEntity.Service1.Id,
            Type = reportEntity.Service1.Type,
            Company = reportEntity.Service1.Company
        }
    }).ToList();

    // Devuelve la lista de DTOs
    return Ok(reportDtos);
}

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] UpdateReportStatusDto dto)
    {
        var reporte = await _context.Reports.FindAsync(id);
        if (reporte == null)
            return NotFound("Reporte no encontrado.");

        reporte.Estado = dto.Estado;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            ok = true,
            message = "Estado actualizado correctamente",
            newStatus = reporte.Estado
        });
    }


    [HttpPost("{id}/evidencias")]
public async Task<IActionResult> UploadEvidencias(int id, List<IFormFile> files)
{
    if (files == null || files.Count == 0)
    {
        return BadRequest("Debes enviar al menos una imagen.");
    }

    if (files.Count > 5)
    {
        return BadRequest("Solo se permiten máximo 5 fotos por reporte.");
    }

    var report = await _context.Reports.FindAsync(id);
    if (report == null)
    {
        return NotFound("Reporte no encontrado.");
    }

    // Crear carpeta si no existe
    var evidenciasPath = Path.Combine(_env.WebRootPath, "Evidencias");
    if (!Directory.Exists(evidenciasPath))
    {
        Directory.CreateDirectory(evidenciasPath);
    }

    var rutasGuardadas = new List<string>();

    foreach (var file in files)
    {
        var extension = Path.GetExtension(file.FileName);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta = Path.Combine(evidenciasPath, nombreArchivo);

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var url = $"/Evidencias/{nombreArchivo}";
        rutasGuardadas.Add(url);

        // Guardar en el reporte
        report.Evidencias!.Add(url);
    }

    _context.Reports.Update(report);
    await _context.SaveChangesAsync();

    return Ok(new
    {
        message = "Evidencias guardadas correctamente.",
        evidencias = rutasGuardadas
    });
}

[HttpGet("{id}/evidencias")]
public async Task<IActionResult> GetEvidencias(int id)
{
    var report = await _context.Reports
        .AsNoTracking()
        .FirstOrDefaultAsync(r => r.Id == id);

    if (report == null)
    {
        return NotFound("Reporte no encontrado.");
    }

    if (report.Evidencias == null || report.Evidencias.Count == 0)
    {
        return Ok(new
        {
            message = "Este reporte no tiene evidencias.",
            evidencias = new List<string>()
        });
    }

    return Ok(new
    {
        message = "Evidencias obtenidas correctamente.",
        evidencias = report.Evidencias
    });
}

}