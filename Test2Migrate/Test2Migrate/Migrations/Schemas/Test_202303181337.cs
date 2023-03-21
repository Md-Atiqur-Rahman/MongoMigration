using MongoDB.Driver;
using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Schemas
{
    public class Test_20230318133701 : IMigration
    {
        public Type Type => typeof(IMigration);
        public async Task UpAsync(IMongoDatabase db)
        {
            throw new NotImplementedException();
        }
    }
}
