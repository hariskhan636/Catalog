using Catalog.Entities;
using Catalog.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Catalog.Repositories
{

    public class MongoDbItemRepository : IItemsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Items> itemsCollection;
        public MongoDbItemRepository(IOptions<MongoDbSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(
            mongoSettings.Value.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Items>(collectionName);
        }
        public async Task CreateItemAsync(Items item)
        {
            await itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Items item)
        {
            await itemsCollection.DeleteOneAsync((x) => x.Id == item.Id);
        }

        public async Task<IEnumerable<Items>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Items> GetItemsAsync(Guid id)
        {
            return await itemsCollection.Find(x => x.Id == id).FirstOrDefaultAsync(); ;
        }

        public async Task UpdateItemAsync(Items item)
        {
            await itemsCollection.ReplaceOneAsync((x => x.Id == item.Id), item);
        }
    }
}