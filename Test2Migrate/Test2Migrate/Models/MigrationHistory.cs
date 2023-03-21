using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Test2Migrate.Models
{
    public class MigrationHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string MigrationClassName { get; set; }
        public string Status { get; set; }
        public DateTime ClassCreationDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
