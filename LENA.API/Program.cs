using LENA.Application.Contracts.Persistence;
using LENA.Application.Repositories;
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowExternal", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(LENA.Application.Features.Wine.Bottles.Commands.CreateBottleCommand).Assembly);
    cfg.AddOpenBehavior(typeof(LENA.Application.Behaviors.LoggingBehavior<,>));
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
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IVintageRepository, VintageRepository>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IFoodFlavorRepository, FoodFlavorRepository>();
builder.Services.AddScoped<IFoodNutrientRepository, FoodNutrientRepository>();
builder.Services.AddScoped<INutrientTypeRepository, NutrientTypeRepository>();
builder.Services.AddScoped<IFlavorProfileRepository, FlavorProfileRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowExternal");

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
