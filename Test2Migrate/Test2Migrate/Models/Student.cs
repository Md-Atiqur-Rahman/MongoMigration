using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Test2Migrate.Models
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        public string? Class { get; set; }
        public string? Type { get; set; }

    }
}
