using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        /// <summary>
        /// Registers data-access services (Dapper/SQL connection factory and repositories).
        /// Applies the process-wide Dapper setting <see cref="DefaultTypeMap.MatchNamesWithUnderscores"/>
        /// so snake_case column names in LENA.Database map to PascalCase CLR properties.
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "A 'DefaultConnection' connection string is required. " +
                    "Add it to LENA.API/appsettings.json or LENA.API/appsettings.Development.json.");
            }

            // Dapper global type-map setting: required because LENA.Database uses snake_case columns.
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(connectionString));

            services.AddScoped<IBottleRepository, BottleRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IMealPlanRepository, MealPlanRepository>();
            services.AddScoped<IGroceryListRepository, GroceryListRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<IVintageRepository, VintageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IFoodFlavorRepository, FoodFlavorRepository>();
            services.AddScoped<IFoodNutrientRepository, FoodNutrientRepository>();
            services.AddScoped<INutrientTypeRepository, NutrientTypeRepository>();
            services.AddScoped<IFlavorProfileRepository, FlavorProfileRepository>();

            return services;
        }
    }
}
