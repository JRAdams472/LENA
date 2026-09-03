using System.Text.Json.Serialization;

using FluentValidation;

using LENA.API.Middleware;
using LENA.API.Services;
using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Infrastructure;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
// Origins allowed to call the API, e.g. "Cors:AllowedOrigins": [ "http://localhost:3000" ].
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "At least one origin must be configured under 'Cors:AllowedOrigins'. " +
        "Add it to LENA.API/appsettings.json or LENA.API/appsettings.Development.json.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowExternal", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .WithHeaders("Accept", "Authorization", "Content-Type")
              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Navigation properties are only populated by the queries that need them.
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<LENA.API.ExceptionHandling.GlobalExceptionHandler>();

// Google-issued JWT bearer token authentication.
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
if (string.IsNullOrWhiteSpace(googleClientId))
{
    throw new InvalidOperationException(
        "A real Google OAuth client id is required under 'Authentication:Google:ClientId'. " +
        "Add it to LENA.API/appsettings.json or LENA.API/appsettings.Development.json, " +
        "or set it via environment variables / user secrets.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.Audience = googleClientId;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuers = JwtConstants.ValidIssuers,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            NameClaimType = "email"
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Every endpoint is secure-by-default; opt out explicitly with [AllowAnonymous] where needed.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddValidatorsFromAssembly(typeof(LENA.Application.IApplicationAssemblyMarker).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(LENA.Application.IApplicationAssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.CachingBehavior<,>));
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.AuditingBehavior<,>));
});

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger is always on in development; it is opt-in via "Swagger:Enabled" outside development.
// When enabled outside development, the Swagger UI and JSON endpoints require an authenticated user.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseExceptionHandler();

app.UseCors("AllowExternal");
app.UseAuthentication();

if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    var isDevelopment = app.Environment.IsDevelopment();
    var swaggerPrefix = (app.Configuration["Swagger:RoutePrefix"] ?? "swagger").Trim('/');

    app.MapOpenApi();

    // Gate the Swagger UI and JSON to authenticated users outside development.
    app.Use(async (context, next) =>
    {
        if (isDevelopment)
        {
            await next();
            return;
        }

        if (context.Request.Path.StartsWithSegments($"/{swaggerPrefix}") &&
            !(context.User.Identity?.IsAuthenticated ?? false))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next();
    });

    app.UseSwagger(options => options.RouteTemplate = $"{swaggerPrefix}/{{documentName}}/swagger.json");
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = swaggerPrefix;
        options.SwaggerEndpoint($"/{swaggerPrefix}/v1/swagger.json", "LENA API v1");
    });
}

app.UseMiddleware<UserResolutionMiddleware>();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();

file static class JwtConstants
{
    public static readonly string[] ValidIssuers = new[] { "https://accounts.google.com", "accounts.google.com" };
}