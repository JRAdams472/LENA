using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace LENA.Application.UnitTests.Database
{
    /// <summary>
    /// Guards the row-count contract used by BaseRepository.ExecuteRequiringMatchAsync.
    /// Any stored procedure invoked through that helper must return the affected row count
    /// as a scalar integer so QuerySingleAsync<int> can detect whether a row matched.
    /// </summary>
    public class StoredProcedureRowCountTests
    {
        private static DirectoryInfo GetDatabaseDirectory()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !Directory.Exists(Path.Combine(directory.FullName, "LENA.Database")))
            {
                directory = directory.Parent;
            }

            directory.Should().NotBeNull("the LENA.Database project must be locatable from the test output directory");
            return directory!;
        }

        private static void AssertReturnsRowCount(string path)
        {
            var sql = File.ReadAllText(path);

            sql.Should().Contain("@@ROWCOUNT", because: $"{path} must capture the affected row count");

            // Normalize whitespace and find the last statement before the trailing END.
            var normalized = Regex.Replace(sql, @"\s+", " ").Trim();
            var lastStatement = Regex.Match(
                normalized,
                @"(?:.*;\s*)?(.*?);\s*END\s*$",
                RegexOptions.Singleline | RegexOptions.IgnoreCase).Groups[1].Value.Trim();

            lastStatement.Should().MatchRegex(
                @"^SELECT\s+(?:@@ROWCOUNT|@\w+)\s*$",
                because: $"{path} must end with a SELECT returning the affected row count so ExecuteRequiringMatchAsync can read it");
        }

        [Fact]
        public void All_Delete_Procedures_Should_Return_RowCount()
        {
            var databaseDirectory = GetDatabaseDirectory();
            var deleteProcedures = Directory.EnumerateFiles(
                Path.Combine(databaseDirectory.FullName, "LENA.Database"),
                "usp_*_Delete.sql",
                SearchOption.AllDirectories);

            deleteProcedures.Should().NotBeEmpty();

            foreach (var path in deleteProcedures)
            {
                AssertReturnsRowCount(path);
            }
        }

        [Fact]
        public void All_Update_Procedures_Should_Return_RowCount()
        {
            var databaseDirectory = GetDatabaseDirectory();
            var updateProcedures = Directory.EnumerateFiles(
                Path.Combine(databaseDirectory.FullName, "LENA.Database"),
                "usp_*_Update.sql",
                SearchOption.AllDirectories);

            updateProcedures.Should().NotBeEmpty();

            foreach (var path in updateProcedures)
            {
                AssertReturnsRowCount(path);
            }
        }
    }
}
