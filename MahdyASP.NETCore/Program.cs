using MahdyASP.NETCore;
using MahdyASP.NETCore.Data;
using MahdyASP.NETCore.Filters;
using MahdyASP.NETCore.Middlewares;
using MahdyASP.NETCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("config.json");

builder.Services.Configure<AttachmentOptions>(
    builder.Configuration.GetSection("Attachments"));

//var attachmentOptions = builder.Configuration.GetSection("Attachments")
//    .Get<AttachmentOptions>();
//builder.Services.TryAddSingleton(attachmentOptions);

//var attachmentOptions = new AttachmentOptions();
//builder.Configuration.GetSection("Attachments").Bind(attachmentOptions);
//builder.Services.TryAddSingleton(attachmentOptions);

// Add services to the container.

builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add<LogActivityFilter>();
        options.Filters.Add<SensitiveActionsLoggerAttribute>();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<IProductsService, ProductsService>();

builder.Services.AddDbContext<ApplicationDBContext>
    (
    cfg => cfg.
    UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
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

app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ProfilingMiddleware>();

app.MapControllers();
app.UseStaticFiles();

app.Run();
