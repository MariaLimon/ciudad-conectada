using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Repositories;
using Backend.Services;
using Backend.Models;
using Backend.Interfaces;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ReporteRepository>();
// Repositorios
builder.Services.AddScoped<IRepository<Report>, ReporteRepository>();
builder.Services.AddScoped<IRepository<Service>, ServiceRepository>();

// Servicios
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IServiceService, ServiceService>();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
public partial class Program { }
