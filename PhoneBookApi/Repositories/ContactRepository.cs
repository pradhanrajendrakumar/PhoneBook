using MongoDB.Bson;
using MongoDB.Driver;
using PhoneBookApi.Models;

namespace PhoneBookApi.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ILogger<ContactRepository> logger;
        private readonly IMongoCollection<Contact> _contactCollection;
        public ContactRepository(IMongoDatabase database, ILogger<ContactRepository> logger)
        {
            _contactCollection = database.GetCollection<Contact>("contacts");
            this.logger = logger;
        }

        public bool DeleteContact(string contactId)
        {
           var result=_contactCollection.DeleteOne(c=>c.Id==contactId);
           return result.DeletedCount> 0;
        }

        public async Task<Contact> GetContactById(string contactId)
        {
            var result = await _contactCollection.FindAsync(c => c.Id == contactId);

            return result.FirstOrDefault();
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
             var allData= await _contactCollection.Find(new BsonDocument()).ToListAsync();
            return allData;
        }

        public async Task InsertContactAsync(Contact contact)
        {
            await _contactCollection.InsertOneAsync(contact);
        }

        public async Task<bool> UpdateContactAsync(Contact contact)
        {
            var filter = Builders<Contact>.Filter.Eq("Id", contact.Id);
            var update = Builders<Contact>.Update
                .Set("FirstName", contact.FirstName)
                .Set("LastName", contact.LastName)
                .Set("Email", contact.Email)
                .Set("PhoneNumber", contact.PhoneNumber)
                .Set("Image", contact.Image);

           var result= await _contactCollection.UpdateOneAsync(filter, update);
           return result.ModifiedCount > 0;
        }
    }
}
