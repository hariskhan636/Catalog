using Catalog.Entities;

namespace Catalog.Repositories
{

    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Items> items = new(){

            new Items { Id= Guid.NewGuid(), Name="Clarity", Price=50, CreatedDate=DateTimeOffset.UtcNow },
            new Items { Id= Guid.NewGuid(), Name="Wraith Band", Price=510, CreatedDate=DateTimeOffset.UtcNow },
            new Items { Id= Guid.NewGuid(), Name="Force Staff", Price=2100, CreatedDate=DateTimeOffset.UtcNow },
            new Items { Id= Guid.NewGuid(), Name="Glimmer Cape", Price=2750, CreatedDate=DateTimeOffset.UtcNow },
        };

        public void CreateItem(Items item)
        {
            items.Add(item);
        }

        public IEnumerable<Items> GetItems()
        {

            return items;
        }

        public Items GetItems(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }

        public void UpdateItem(Items item)
        {
            var index = items.FindIndex(i => i.Id == item.Id);
            items[index] = item;
        }
    }
}