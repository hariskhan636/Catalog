using Catalog.Dtos;
using Catalog.Entities;

namespace Catalog
{
    public static class Extensions
    {

        public static ItemDto AsDto(this Items item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);

        }
    }
}