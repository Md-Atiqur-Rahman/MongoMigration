using MongoDB.Driver;
using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Schemas
{
    public class Migration_202303161017 : IMigration
    {
        private readonly IMongoDatabase _db;
        public Migration_202303161017(IMongoDatabase db)
        {
            _db = db;
        }
        public Type Type => typeof(IMigration);
        public async Task UpAsync()
        {
            var collection = _db.GetCollection<Student>("studentCourses");
            await collection.InsertOneAsync(new Student
            {
                Name = "Test1",
                Type = "AddedInMigration1"
            });
        }
    }
}
