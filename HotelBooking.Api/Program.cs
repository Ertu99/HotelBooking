using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using HotelBooking.Api.BackgroundTasks;
using HotelBooking.Api.Middlewares;
using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Mappings;
using HotelBooking.Application.Services;
using HotelBooking.Application.Validations;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Domain.Repositories;
using HotelBooking.Infrastructure.Persistence;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// JWT
// --------------------------------------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
    };
});

// --------------------------------------------------
// Serilog
// --------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

// --------------------------------------------------
// CORS
// --------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// --------------------------------------------------
// Controllers & Swagger
// --------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HotelBooking.Api",
        Version = "v1"
    });

    // JWT için
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// --------------------------------------------------
// Validation
// --------------------------------------------------
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<CreateHotelDtoValidator>();

// --------------------------------------------------
// API Versioning
// --------------------------------------------------
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// --------------------------------------------------
// Hangfire
// --------------------------------------------------
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlServer"),
              new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.FromSeconds(15),
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              });
});

builder.Services.AddHangfireServer();

// --------------------------------------------------
// OpenAPI
// --------------------------------------------------
builder.Services.AddOpenApi();

// --------------------------------------------------
// DbContext
// --------------------------------------------------
var cs = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<HotelBookingDbContext>(opt =>
    opt.UseSqlServer(cs));

// --------------------------------------------------
// Service Registrations
// --------------------------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHostedService<LogWriterService>();

// --------------------------------------------------
// Build
// --------------------------------------------------
var app = builder.Build();

// --------------------------------------------------
// 🔥 MIDDLEWARE ORDER (En önemli kısım)
// --------------------------------------------------

// 1) Custom Auth Middleware (HER ŞEYDEN ÖNCE)
app.UseMiddleware<AuthMiddleware>();

// 2) Request Logging
app.UseMiddleware<RequestLoggingMiddleware>();

// 3) CORS
app.UseCors("AllowFrontend");

// 4) Global Exception Handler
app.UseMiddleware<ExceptionMiddleware>();

// 5) Swagger (dev ortamı)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6) HTTPS
app.UseHttpsRedirection();

// 7) Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 8) Routing (Controllers)
app.MapControllers();

// 9) Hangfire Dashboard (SONDA OLMALI)
app.UseHangfireDashboard("/hangfire");

// --------------------------------------------------
app.Run();
public partial class Program { }