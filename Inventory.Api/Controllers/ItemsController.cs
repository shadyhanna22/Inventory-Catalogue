using Inventory.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Inventory.Api.Dtos;
using Inventory.Api.Entities;

namespace Inventory.Api.Controllers
{
    //make as a controller
    [ApiController]
    //can use [Route("[controller]")]
    // /items
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;
        private readonly ILogger<ItemsController> logger;

        public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        //GET / items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string nameToMatch = null)
        {
            var items = (await repository.GetItemsAsync())
                        .Select(item => item.AsDto());
            if (!string.IsNullOrWhiteSpace(nameToMatch))
            {
                items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");

            return items;
        }

        //GET / items/{id}
        [HttpGet("{id}")]
        //ActionResult to returen more than one type (Item, NotFound)
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreatedItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreatedItemAsync(item);

            //CreateAtRoute
            return CreatedAtAction(nameof(GetItemAsync), new{id = item.Id}, item.AsDto());
        }

        // POST /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id,UpdateItemDto itemDto)
        {
            var exisitingItem = await repository.GetItemAsync(id);

            if (exisitingItem is null){
                return NotFound();
            }

            exisitingItem.Name = itemDto.Name;
            exisitingItem.Price = itemDto.Price;

            await repository.UpdateItemAsync(exisitingItem);
            return NoContent();
        }

        //DELETE/items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id){
            var exisitingItem = await repository.GetItemAsync(id);

            if (exisitingItem is null){
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}