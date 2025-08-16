using Luma.API.Configuration;
using Luma.API.Domain.Entities;
using Luma.API.Infrastructure;
using Luma.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LumaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LumaDb"),
    npg => npg.MapEnum<Breakpoint>()
    )
    .UseSnakeCaseNamingConvention()
);

builder.Services.Configure<GoogleApiSettings>(
    builder.Configuration.GetSection("GoogleApis"));
builder.Services.Configure<GoogleApiSettings.WeatherSettings>(
    builder.Configuration.GetSection("GoogleApis:Weather"));
builder.Services.Configure<GoogleApiSettings.GeocodingSettings>(
    builder.Configuration.GetSection("GoogleApis:Geocoding"));

builder.Services.AddHttpClient<IWeatherService, GoogleWeatherService>();
builder.Services.AddHttpClient<IGeocodingService, GoogleGeocodingService>();

builder.Services.AddHttpClient();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Generates the swagger/v1/swagger.json file
    app.UseSwagger();

    // Enables the Swagger UI page
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Luma API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
