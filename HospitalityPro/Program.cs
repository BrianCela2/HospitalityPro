using DI;
using Domain.Mappings;
using Entities.Models;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HospitalityProContext>(options => options.UseSqlServer(connString));



// Add services to the container.

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddJsonOptions(p => p.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(GeneralProfile));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
});

// use Lamar as DI.
builder.Host.UseLamar((context, registry) =>
{
    // register services using Lamar
    registry.IncludeRegistry<HospitalityRegistry>();
    registry.IncludeRegistry<MapperRegistry>();
    // add the controllers
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();