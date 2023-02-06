using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PhoneBookApi.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string? LastName { get; set; }
        [BsonElement("Email")]
        public string? Email { get; set; }
        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("Image")]
        public string? Image { get; set; }
    }
}
