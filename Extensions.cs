using Catalog.DTOs;
using Catalog.Entities;

namespace Catalog
{
    public static class Extensions
    {

        public static ItemDto AsDto(this Items item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };
        }
    }
}