using SparePackaging.Application.Interfaces;
using SparePackaging.Application.Mappings;
using SparePackaging.Application.Services;
using SparePackaging.Domain.Interfaces;
using SparePackaging.Infrastructure.Persistence;
using SparePackaging.Infrastructure.Repositories;
using SparePackaging.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Oracle Connection Factory
builder.Services.AddSingleton<OracleConnectionFactory>();

// Repositories
builder.Services.AddScoped<IVendorItemRepository,VendorItemRepository>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork,OracleUnitOfWork>();

// Application Services
builder.Services.AddScoped<IVendorItemService,VendorItemService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
builder.Services.AddScoped<OracleDbSession>();
var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
