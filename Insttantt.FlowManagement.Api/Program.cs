using Insttantt.FlowManagement.Api;
using Insttantt.FlowManagement.Application.Middleware;
using Insttantt.FlowManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


string title = "Insttants.FlowManagement.Api";
string version = "1.0";
string description = $"{title} {version} - Initial Version.";

var configuration = new ConfigurationBuilder()
    .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
});

//Add ExceptionHandler
builder.Services.AddExceptionHandler<ExceptionHandler>();


//Add DbContext
var connectionString = configuration.GetConnectionString("InsttanttDb");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Add Dependency Injection
builder.Services.AddDependency();

//AddSerilog
builder.Host
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
        configuration.Enrich.FromLogContext();
    });

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", description);
    });
}

//Add ExceptionHandler
app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

//logger for the readable log records 
app.UseSerilogRequestLogging();
app.UseDeveloperExceptionPage();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();


app.MapControllers();

app.Run();
