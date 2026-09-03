using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;

using Xunit;

namespace LENA.IntegrationTests
{
    public sealed class DatabaseFixture : IAsyncLifetime
    {
        static DatabaseFixture()
        {
            // Required: LENA.Database uses snake_case columns; Dapper must map them to PascalCase properties.
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        private const string BaseConnectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;TrustServerCertificate=true";
        private string _databaseName = $"LENA_Integration_{Guid.NewGuid():N}";
        private string _dacpacPath = string.Empty;

        public bool IsAvailable { get; private set; }
        public string? ConnectionString { get; private set; }

        public async Task InitializeAsync()
        {
            _databaseName = $"LENA_Integration_{Guid.NewGuid():N}";
            _dacpacPath = LocateDacpac();

            try
            {
                await using var connection = new SqlConnection(BaseConnectionString);
                await connection.OpenAsync();
                IsAvailable = true;
            }
            catch
            {
                IsAvailable = false;
                return;
            }

            await CreateDatabaseAsync();
            await DeployDacpacAsync();

            ConnectionString = $"{BaseConnectionString};Database={_databaseName}";
        }

        public async Task DisposeAsync()
        {
            if (!IsAvailable) return;

            try
            {
                await using var connection = new SqlConnection(BaseConnectionString);
                await connection.OpenAsync();
                await connection.ExecuteAsync($"ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE IF EXISTS [{_databaseName}];");
            }
            catch
            {
                // best-effort cleanup
            }
        }

        private async Task CreateDatabaseAsync()
        {
            await using var connection = new SqlConnection(BaseConnectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync($"CREATE DATABASE [{_databaseName}]");
        }

        private async Task DeployDacpacAsync()
        {
            if (string.IsNullOrEmpty(_dacpacPath))
                throw new InvalidOperationException("Could not locate LENA.Database.dacpac");

            var connectionString = $"{BaseConnectionString};Database={_databaseName}";
            var services = new DacServices(connectionString);
            using var package = DacPackage.Load(_dacpacPath);

            var options = new DacDeployOptions
            {
                CreateNewDatabase = false,
                BlockOnPossibleDataLoss = false
            };

            await Task.Run(() => services.Deploy(package, _databaseName, upgradeExisting: true, options));
        }

        private static string LocateDacpac()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && directory.GetFiles("*.slnx", SearchOption.TopDirectoryOnly).Length == 0)
            {
                directory = directory.Parent;
            }

            if (directory is null)
                return string.Empty;

            var dacpac = directory
                .GetFiles("LENA.Database.dacpac", SearchOption.AllDirectories)
                .OrderByDescending(f => f.LastWriteTimeUtc)
                .FirstOrDefault();

            return dacpac?.FullName ?? string.Empty;
        }
    }
}