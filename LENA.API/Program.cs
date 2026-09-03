using FluentValidation;
using System.Text.Json.Serialization;
using LENA.API.Middleware;
using LENA.API.Services;
using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        new CompactJsonFormatter(),
        "logs/log-.json",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 31,
        fileSizeLimitBytes: 10485760,
        rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger, true);

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
            ValidIssuers = new[] { "https://accounts.google.com", "accounts.google.com" },
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

builder.Services.AddValidatorsFromAssembly(typeof(LENA.Application.Features.Wine.Bottles.Commands.CreateBottleCommand).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(LENA.Application.Features.Wine.Bottles.Commands.CreateBottleCommand).Assembly);
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.AuditingBehavior<,>));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "A 'DefaultConnection' connection string is required. " +
        "Add it to LENA.API/appsettings.json or LENA.API/appsettings.Development.json.");
}

builder.Services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(connectionString));

builder.Services.AddScoped<IBottleRepository, BottleRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IMealPlanRepository, MealPlanRepository>();
builder.Services.AddScoped<IGroceryListRepository, GroceryListRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IVintageRepository, VintageRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IFoodFlavorRepository, FoodFlavorRepository>();
builder.Services.AddScoped<IFoodNutrientRepository, FoodNutrientRepository>();
builder.Services.AddScoped<INutrientTypeRepository, NutrientTypeRepository>();
builder.Services.AddScoped<IFlavorProfileRepository, FlavorProfileRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger is always on in development; it is opt-in via "Swagger:Enabled" outside development.
// When enabled outside development, protect it from public access (e.g., restrict the reverse proxy or add authorization).
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    var swaggerPrefix = (app.Configuration["Swagger:RoutePrefix"] ?? "swagger").Trim('/');

    app.MapOpenApi();
    app.UseSwagger(options => options.RouteTemplate = $"{swaggerPrefix}/{{documentName}}/swagger.json");
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = swaggerPrefix;
        options.SwaggerEndpoint($"/{swaggerPrefix}/v1/swagger.json", "LENA API v1");
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseExceptionHandler();

app.UseCors("AllowExternal");

app.UseAuthentication();
app.UseMiddleware<UserResolutionMiddleware>();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
