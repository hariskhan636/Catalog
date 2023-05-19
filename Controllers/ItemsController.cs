using Catalog.DTOs;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]

        public ActionResult<ItemDto> GetItems(Guid id)
        {

            var item = repository.GetItems(id);

            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }


        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {

            Items item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item.AsDto());
        }
    }

}