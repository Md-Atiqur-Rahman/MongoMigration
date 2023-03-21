using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Locators
{
    public interface IMigrationLocator<TMigrationType> where TMigrationType : class, IMigration
    {
        IEnumerable<TMigrationType> GetMigrations(Type type, List<MigrationHistory> appliedData);
        IEnumerable<TMigrationType> GetMigrationByName(Type type, string runPendingFile);
        void Locate(List<MigrationHistory> appliedData);
        void Locate2(string runMigration);
    }
}
