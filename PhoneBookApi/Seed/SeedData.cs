using MongoDB.Bson;
using MongoDB.Driver;
using PhoneBookApi.Models;

namespace PhoneBookApi.Seed
{
    public class SeedData
    {
        private readonly IMongoCollection<Contact> _collection;

        public SeedData(IMongoDatabase database)
        {
            _collection = database.GetCollection<Contact>("contacts");
        }

        public void EnsureSeedData()
        {
            if (_collection.CountDocuments(new BsonDocument()) == 0)
            {
                List<Contact> seedData = new List<Contact>
                {
                    new Contact
                    {
                        FirstName="John",LastName="Doe",Email="john.doe@example.com",PhoneNumber="555-555-5555",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Jane",LastName="Smith",Email="jane.smith@example.com",PhoneNumber="555-555-5556",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Bob",LastName="Johnson",Email="bob.johnson@example.com",PhoneNumber="555-555-5557",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Alice",LastName="Brown",Email="alice.brown@example.com",PhoneNumber="555-555-5558",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Michael",LastName="Davis",Email="michael.davis@example.com",PhoneNumber="555-555-5559",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="William",LastName="Anderson",Email="william.anderson@example.com",PhoneNumber="555-555-5560",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Emily",LastName="Wilson",Email="emily.wilson@example.com",PhoneNumber="555-555-5561",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Ashley",LastName="Thomas",Email="ashley.thomas@example.com",PhoneNumber="555-555-5562",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Brian",LastName="Jackson",Email="brian.jackson@example.com",PhoneNumber="555-555-5563",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Jennifer",LastName="White",Email="jennifer.white@example.com",PhoneNumber="555-555-5564",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Elizabeth",LastName="Harris",Email="elizabeth.harris@example.com",PhoneNumber="555-555-5565",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="David",LastName="Martin",Email="david.martin@example.com",PhoneNumber="555-555-5566",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Sarah",LastName="Thompson",Email="sarah.thompson@example.com",PhoneNumber="555-555-5567",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Christopher",LastName="Young",Email="christopher.young@example.com",PhoneNumber="555-555-5568",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Matthew",LastName="Allen",Email="matthew.allen@example.com",PhoneNumber="555-555-5569",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Lauren",LastName="King",Email="lauren.king@example.com",PhoneNumber="555-555-5570",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Kevin",LastName="Wright",Email="kevin.wright@example.com",PhoneNumber="555-555-5571",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Rachel",LastName="Scott",Email="rachel.scott@example.com",PhoneNumber="555-555-5572",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Jane",LastName="Smith",Email="jane.smith@example.com",PhoneNumber="555-555-5556",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Jason",LastName="Green",Email="jason.green@example.com",PhoneNumber="555-555-5573",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    },
                    new Contact
                    {
                        FirstName="Jessica",LastName="Baker",Email="jessica.baker@example.com",PhoneNumber="555-555-5574",Image="https://www.gstatic.com/webp/gallery/1.jpg"
                    }
                };

                _collection.InsertMany(seedData);
            }
        }
    }
}
