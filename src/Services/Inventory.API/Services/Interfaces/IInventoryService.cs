using Inventory.API.Entities;
using Inventory.API.Repositories.Abstraction;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.API.Services.Interfaces
{
    public interface IInventoryService : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);

        Task<PagedList<InventoryEntryDto>> GetAllByItemPagingAsync(GetInventoryPagingQuery query);

        Task<InventoryEntryDto> GetByIdAsync(string id);

        Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
    }
}
