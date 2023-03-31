using FluentValidation;
using Microsoft.OpenApi.Models;
using Saiketsu.Service.User.Application;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Domain.Options;
using Saiketsu.Service.User.Infrastructure.Services;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

static void InjectSerilog(WebApplicationBuilder builder)
{
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());
}

void AddServices(WebApplicationBuilder builder)
{
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));
    builder.Services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "User API",
            Description = ".NET Web API for managing Auth0 users."
        });

        options.EnableAnnotations();
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
            "Saiketsu.Service.User.Application.xml"));
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Saiketsu.Service.User.Domain.xml"));
    });

    builder.Services.Configure<Auth0Options>(builder.Configuration.GetSection(Auth0Options.Position));
    builder.Services.Configure<Auth0ManagementOptions>(
        builder.Configuration.GetSection(Auth0ManagementOptions.Position));

    builder.Services.AddSingleton<Auth0TokenService>();
    builder.Services.AddScoped<IAuth0Service, Auth0Service>();
}

void AddMiddleware(WebApplication app)
{
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
}

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    InjectSerilog(builder);
    AddServices(builder);

    var app = builder.Build();

    AddMiddleware(app);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}