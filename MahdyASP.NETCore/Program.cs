using MahdyASP.NETCore.Data;
using MahdyASP.NETCore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<IProductsService, ProductsService>();

builder.Services.AddDbContext<ApplicationDBContext>(
    builder => builder.UseSqlServer("Server=db4327.public.databaseasp.net; Database=db4327; User Id=db4327; Password=a!3ZLc?5t6N+; Encrypt=False; MultipleActiveResultSets=True;")
    );

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
