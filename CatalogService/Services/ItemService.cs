using System;
using AutoMapper;
using CatalogService.Contracts;
using CatalogService.Data;
using CatalogService.Interfaces;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services;

public class ItemService : IItemService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ItemService> _logger;
    private readonly IMapper _mapper;

    public ItemService(AppDbContext context, ILogger<ItemService> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateItemAsync(CreateItemRequest request)
    {
        try
        {
            var item = _mapper.Map<Item>(request);
            if (!_context.Categories.Any(x => x.CategoryId == request.CategoryId))
            {
                throw new KeyNotFoundException($"The referencec Category Id {request.CategoryId} does not exist.");
            }
            item.CreatedAt = DateTime.UtcNow;
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"The referencec Category Id {request.CategoryId} does not exist.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the Item.");
            throw;
        }
    }

    public async Task<Item> GetItemByIdAsync(int id)
    {
        try
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"No Item with Id {id} found.");
            }
            return item;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"No Item with Id {id} found.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the Item with id {id}.");
            throw;
        }
    }

    public async Task<IEnumerable<Item>> GetItemsByCategoryAsync(int categoryId, int page, int pageSize)
    {
        try
        {
            var items = await _context.Items.Where(x => x.CategoryId == categoryId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the Item with Category id {categoryId}.");
            throw;
        }
    }

    public async Task UpdateItemAsync(int itemId, UpdateItemRequest request)
    {
        try
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with id {itemId} not found.");
            }

            if (request.Name != null)
            {
                item.Name = request.Name;
            }

            if (request.Description != null)
            {
                item.Description = request.Description;
            }

            item.CategoryId = request.CategoryId;
            item.Price = request.Price;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"Item with id {itemId} not found.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the Item with id {itemId}.");
            throw;
        }
    }

    public async Task DeleteItemAsync(int id)
    {
        try
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No  item found with the id {id}");
            }
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"No Item found with the id {id}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the Item with id {id}.");
            throw;
        }
    }
}
