using MongoDB.Driver;
using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Schemas
{
    public class Migration_202303161021 : IMigration
    {
        public Type Type => typeof(IMigration);
        public async Task UpAsync(IMongoDatabase db)
        {
            var collection = db.GetCollection<Student>("studentCourses");
            await collection.InsertOneAsync(new Student
            {
                Name = "Test2",
                Type = "AddedInMigration"
            });
        }
    }
}
