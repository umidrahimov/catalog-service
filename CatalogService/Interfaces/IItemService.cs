using System;
using CatalogService.Contracts;
using CatalogService.Models;

namespace CatalogService.Interfaces;

public interface IItemService
{
    Task<IEnumerable<Item>> GetItemsByCategoryAsync(int id, int page, int pageSize);
    Task<Item> GetItemByIdAsync(int id);
    Task CreateItemAsync(CreateItemRequest request);
    Task UpdateItemAsync(int id, UpdateItemRequest request);
    Task DeleteItemAsync(int id);
}
