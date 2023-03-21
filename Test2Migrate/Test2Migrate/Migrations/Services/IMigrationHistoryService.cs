using MongoDB.Driver;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Services
{
    public interface IMigrationHistoryService
    {
        Task Save(IMigration migration);
        List<MigrationHistory> GetAllAppliedMigartionHistory();
        List<MigrationHistory> GetAppliedMigartionFileNames();
        List<MigrationHistory> GetPendingMigartionFileNames();
        bool IsApplied(string migrationFileName);
    }
}
