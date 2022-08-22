using Microsoft.AspNetCore.Mvc;
using Sigma.IOT.API.Repositories.Base.Azure;
using Sigma.IOT.API.Repositories.Forecast;
using Sigma.IOT.API.Services.Forecast;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddScoped<IForecastService, ForecastService>();
builder.Services.AddScoped<IForecastStorageRepository, ForecastStorageRepository>();
builder.Services.AddScoped<IStorageRepository, StorageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiVersioning();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
