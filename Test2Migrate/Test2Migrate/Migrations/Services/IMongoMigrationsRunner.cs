namespace Test2Migrate.Migrations.Services
{
    public interface IMongoMigrationsRunner
    {
     public Task RunMigrationByFileName(string migrationName);
     public Task RunAllPendingMigration();
    }
}
