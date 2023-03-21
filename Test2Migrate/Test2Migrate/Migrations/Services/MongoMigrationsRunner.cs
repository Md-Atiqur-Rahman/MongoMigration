using MongoDB.Driver;
using Test2Migrate.Connection;
using Test2Migrate.Migrations.Locators;

namespace Test2Migrate.Migrations.Services
{
    public class MongoMigrationsRunner : IMongoMigrationsRunner
    {
        private readonly Type DatabaseMigrationType = typeof(IMigration);
        private readonly IMongoDatabase _db;
        private readonly IMigrationHistoryService _migrationHistoryService;
        private readonly ILogger _logger;
        private IDatabaseTypeMigrationDependencyLocator _migrationLocator { get; }

        public MongoMigrationsRunner(
            IDatabaseSettings settings,
            ILoggerFactory loggerFactory,
            IDatabaseTypeMigrationDependencyLocator migrationLocator,
            IMigrationHistoryService migrationHistoryService,
            IMongoClient client)
        {
            _migrationLocator = migrationLocator;
            _db = client.GetDatabase(settings.DatabaseName);
            _logger = loggerFactory.CreateLogger<MongoMigrationsRunner>();
            _migrationHistoryService = migrationHistoryService;
        }
        
        public async Task RunAllPendingMigration()
        {
            await MigrateUp(_db);
        }


        private async Task MigrateUp(IMongoDatabase db)
        {
            var appliedMigrtionData = _migrationHistoryService.GetAppliedMigartionFileNames();
            var migrations = _migrationLocator.GetMigrations(DatabaseMigrationType, appliedMigrtionData).ToList();

            foreach (var migration in migrations)
            {
                //_logger.LogInformation("Database Migration Up: {0}:{1} ", DatabaseMigrationType.GetType().ToString());

              await migration.UpAsync(db);
              await _migrationHistoryService.Save(migration);

               // _logger.LogInformation("Database Migration Up finished successful: {0}:{1} ", migration.GetType().ToString());
            }
        }

        public async Task RunMigrationByFileName(string migrationName)
        {
            await MigrationByFileName(migrationName, _db);
        }
        public async Task MigrationByFileName(string migrationName, IMongoDatabase db)
        {
            var isApplied = _migrationHistoryService.IsApplied(migrationName);

            var migrations = _migrationLocator.GetMigrationByName(DatabaseMigrationType, migrationName).ToList();

            if(!isApplied)
            {
                foreach (var migration in migrations)
                {
                    //_logger.LogInformation("Database Migration Up: {0}:{1} ", DatabaseMigrationType.GetType().ToString());

                    await migration.UpAsync(db);
                    await _migrationHistoryService.Save(migration);

                    // _logger.LogInformation("Database Migration Up finished successful: {0}:{1} ", migration.GetType().ToString());
                }
            }
            
        }



    }
}
