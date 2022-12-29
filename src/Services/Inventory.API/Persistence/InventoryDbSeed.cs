using Inventory.API.Entities;
using Inventory.API.Extensions;
using MongoDB.Driver;

namespace Inventory.API.Persistence
{
    public class InventoryDbSeed
    {
        public async Task SeedDataAsync(IMongoClient mongoClient, DatabaseSettings settings)
        {
            var databaseName = settings.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventoryEntry>("InventoryEntries");
            if(await inventoryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await inventoryCollection.InsertManyAsync(GetPerconfiguredInventoryEntries());
            }
        }

        private static IEnumerable<InventoryEntry> GetPerconfiguredInventoryEntries()
        {
            return new List<InventoryEntry>()
            {
                new InventoryEntry()
                {
                    ItemNo = "Name Fuck bitch1",
                    DocumentNo = Guid.NewGuid().ToString(),
                    ExternalDocumentNo = Guid.NewGuid().ToString(),
                    Quantity = 10,
                    DocumentType = Shared.Enums.EDocumentType.Purchase
                },
                new InventoryEntry()
                {
                    ItemNo = "Name Fuck bitch",
                    DocumentNo = Guid.NewGuid().ToString(),
                    ExternalDocumentNo = Guid.NewGuid().ToString(),
                    Quantity = 17,
                    DocumentType = Shared.Enums.EDocumentType.Purchase
                }
            };
        }
    }
}
