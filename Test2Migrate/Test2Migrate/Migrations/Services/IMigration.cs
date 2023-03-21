using MongoDB.Driver;

namespace Test2Migrate.Migrations.Services
{
    public interface IMigration
    {
        Type Type { get; }
        public  Task UpAsync(IMongoDatabase db);
    }
}
