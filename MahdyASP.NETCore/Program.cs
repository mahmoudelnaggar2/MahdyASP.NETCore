using System.Security.Claims;
using System.Text;
using MahdyASP.NETCore;
using MahdyASP.NETCore.Authentication;
using MahdyASP.NETCore.Authorization;
using MahdyASP.NETCore.Data;
using MahdyASP.NETCore.Filters;
using MahdyASP.NETCore.Middlewares;
using MahdyASP.NETCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

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
        options.Filters.Add<PermissionBasedAuthorizationFilter>();
        options.Filters.Add<LogActivityFilter>();
        options.Filters.Add<SensitiveActionsLoggerAttribute>();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
//builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions,
//BasicAuthenticationHandler>("Basic", null);

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddSingleton<IAuthorizationHandler, AgeAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeesOnly", builder =>
    {
        builder.RequireClaim("UserType", "Employee");
    });

    options.AddPolicy("SuperUsers", builder =>
    {
        builder.RequireRole("Admin");
        builder.RequireRole("SuperAdmin");
    });

    options.AddPolicy("AgeGreaterThan25", b =>
    {
        b.RequireAssertion(c =>
        {
            var dob = DateTime.Parse(c.User.FindFirstValue("DateOfBirth"));

            return DateTime.Today.Year - dob.Year >= 25;
        });
    });

    options.AddPolicy("AgeGreaterThan25Requirements",
        b => b.AddRequirements(new AgeGreaterThan25Requirement()));
});

builder.Services.AddAuthentication().AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audiance,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };
    }
);

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
