using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Repositories;
using Backend.Services;
using Backend.Models;
using Backend.Interfaces;
using DotNetEnv;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ==========================
//  JWT
// ==========================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");
var issuer = jwtSettings.GetValue<string>("Issuer");
var audience = jwtSettings.GetValue<string>("Audience");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// ======================
// CORS
// ======================
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(
            "http://127.0.0.1:5501",
            "http://localhost:5501",
            "https://127.0.0.1:5501",
            "https://localhost:5501"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddControllers();

// ======================
// Repos y Servicios
// ======================
builder.Services.AddScoped<ReporteRepository>();
builder.Services.AddScoped<IRepository<Report>, ReporteRepository>();
builder.Services.AddScoped<IRepository<Service>, ServiceRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IServiceService, ServiceService>();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// ======================
// SWAGGER
// ======================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token así → Bearer {TOKEN}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ======================
// CONSTRUIR APP
// ======================
var app = builder.Build();

// ======================
// RUTEO Y MIDDLEWARES
// ======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

// IMPORTANTE: ESTO VA AQUÍ, NO EN ADDSwaggerGEN
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
    ),
    RequestPath = ""
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
