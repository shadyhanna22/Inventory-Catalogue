// using System.Collections.Generic;
// using Inventory.Api.Entities;

// namespace Inventory.Api.Repositories
// {
//     public class InMemItemsRepository : IItemsRepository
//     {
//         //set items
//         private readonly List<Item> items = new()
//         {
//             new Item { Id = Guid.NewGuid(), Name = "Hat", Price = 15, CreatedDate = DateTimeOffset.UtcNow},
//             new Item { Id = Guid.NewGuid(), Name = "Hat", Price = 15, CreatedDate = DateTimeOffset.UtcNow},
//             new Item { Id = Guid.NewGuid(), Name = "Hat", Price = 15, CreatedDate = DateTimeOffset.UtcNow},
//             new Item { Id = Guid.NewGuid(), Name = "Hat", Price = 15, CreatedDate = DateTimeOffset.UtcNow}
//         };

//         //return a collection of items
//         public IEnumerable<Item> GetItemsAsync()
//         {
//             return items;
//         }

//         //return one item
//         public Item GetItemAsync(Guid id)
//         {
//             return items.Where(item => item.Id == id).SingleOrDefault();
//         }

//         public void CreatedItemAsync(Item item)
//         {
//             items.Add(item);
//         }

//         public void UpdateItemAsync(Item item)
//         {
//             var index = items.FindIndex(exisitingItem => exisitingItem.Id == item.Id);
//             items[index] = item;
//         }

//         public void DeleteItemAsync(Guid id)
//         {
//             var index = items.FindIndex(exisitingItem => exisitingItem.Id == id);
//             items.RemoveAt(index);
//         }
//     }
// }
