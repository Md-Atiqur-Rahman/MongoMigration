using MongoDB.Driver;
using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Schemas
{
    public class MyEntity_20230319124219 : IMigration
    {
       
        public Type Type => typeof(IMigration);
        public async Task UpAsync(IMongoDatabase _db)
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
