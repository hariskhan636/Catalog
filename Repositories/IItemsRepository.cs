using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        IEnumerable<Items> GetItems();
        Items GetItems(Guid id);

        void CreateItem(Items item);
        void UpdateItem(Items item);
    }
}