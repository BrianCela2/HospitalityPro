using DI;
using Domain.Mappings;
using Entities.Models;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Domain.Notifications;
using Microsoft.AspNetCore.Http.Connections;


var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HospitalityProContext>(options => { options.UseSqlServer(connString); options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); });

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddJsonOptions(p => p.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
}); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(GeneralProfile));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", b =>
        b.AllowAnyMethod()
         .AllowAnyHeader()
         .WithOrigins("http://localhost:4200") // Update with your Angular app's origin
         .AllowCredentials());
});

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
	opt.RequireHttpsMetadata = false; 
	opt.SaveToken = true;
	opt.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				builder.Configuration.GetSection("AppSettings:Token").Value!)),
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidateAudience = true,
		ValidAudience = builder.Configuration["JWT:Audience"],
	};
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "HospitalityPro",
		Version = "v1"
	});
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer jhfdkj.jkdsakjdsa.jkdsajk\"",
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});


// use Lamar as DI.
builder.Host.UseLamar((context, registry) =>
{
    // register services using Lamar
    registry.IncludeRegistry<HospitalityRegistry>();
    registry.IncludeRegistry<MapperRegistry>();
    // add the controllers
});

builder.Services.AddDistributedMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.MapHub<NotificationHub>("/Notify", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
}
);
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<RequestResponseLoggerMiddleware>();

app.MapControllers();

app.Run();
