using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        Task<IEnumerable<Items>> GetItemsAsync();
        Task<Items> GetItemsAsync(Guid id);

        Task CreateItemAsync(Items item);
        Task UpdateItemAsync(Items item);

        Task DeleteItemAsync(Items item);
    }
}