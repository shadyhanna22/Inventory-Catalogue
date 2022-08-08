using Inventory.Api.Dtos;
using Inventory.Api.Entities;

namespace Inventory.Api
{
    public static class Extentions{
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id,item.Name,item.Description,item.Price,item.CreatedDate);
        }
    }
}