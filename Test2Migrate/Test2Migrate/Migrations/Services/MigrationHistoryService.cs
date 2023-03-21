using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using Test2Migrate.Connection;
using Test2Migrate.Migrations.ExtensionMethods;
using Test2Migrate.Migrations.Locators;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Services
{
    public class MigrationHistoryService : IMigrationHistoryService
    {
        private readonly IMongoDatabase _db;
        private readonly Type DatabaseMigrationType = typeof(IMigration);
        private IDatabaseTypeMigrationDependencyLocator _migrationLocator { get; }
        public MigrationHistoryService(
            IDatabaseSettings settings,
            IDatabaseTypeMigrationDependencyLocator migrationLocator,
            IMongoClient client)
        {
            _migrationLocator = migrationLocator;
            _db = client.GetDatabase(settings.DatabaseName);
        }

        public bool IsApplied(string migrationFileName)
        {
            var collection = _db.GetCollection<MigrationHistory>("migrationHistory");
            bool exists = collection.Find(x => x.MigrationClassName == migrationFileName).Any();
            return exists;
        }
        public List<MigrationHistory> GetAllAppliedMigartionHistory()
        {
            var collection = _db.GetCollection<MigrationHistory>("migrationHistory");
            var getMigratedData = collection.Find(k => true).ToList();
            return getMigratedData;
        }
        public List<MigrationHistory> GetAppliedMigartionFileNames()
        {
            var collection = _db.GetCollection<MigrationHistory>("migrationHistory");
            
            var projection = Builders<MigrationHistory>
                    .Projection.Include(x => x.MigrationClassName).Exclude(x=>x.Id);

            var getMigratedData = collection.Find(k => true)
                .Project<MigrationHistory>(projection).ToList();
            return getMigratedData;
        }
        public List<MigrationHistory> GetPendingMigartionFileNames()
        {
            List<MigrationHistory> pendingFiles = new List<MigrationHistory>();
           var appliedMigrtionData = GetAppliedMigartionFileNames();
            var migrations = _migrationLocator.GetMigrations(DatabaseMigrationType, appliedMigrtionData).ToList();
            foreach (var migration in migrations)
            {
                string migrationClass;
                DateTime classCreationDate;
                GetMigrationCreationDate(migration, out migrationClass, out classCreationDate);
                MigrationHistory obj = new MigrationHistory();
                obj.MigrationClassName = migrationClass;
                obj.ClassCreationDate = classCreationDate;
                obj.Status = MigrationStatus.Pending;
                pendingFiles.Add(obj);
            }
            return pendingFiles;
        }

        private static void GetMigrationCreationDate(IMigration? migration, out string migrationClass, out DateTime classCreationDate)
        {
            migrationClass = migration.GetType().Name.ToString();
            string[] splitMigrationClass = migrationClass.Split("_");
            string date = splitMigrationClass[1];
            classCreationDate = DateTime.ParseExact(date, "yyyyMMddHHmmss",
            System.Globalization.CultureInfo.InvariantCulture);
        }

        public async Task Save(IMigration migration)
        {
            try
            {
                string migrationClass;
                DateTime classCreationDate;
                GetMigrationCreationDate(migration, out migrationClass, out classCreationDate);
                var collection = _db.GetCollection<MigrationHistory>("migrationHistory");
                collection.InsertOne(new MigrationHistory
                {
                    MigrationClassName = migrationClass,
                    ClassCreationDate = classCreationDate,
                    CreatedDate = DateTime.Now,
                    Status = MigrationStatus.Applied
                });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
    }
}
