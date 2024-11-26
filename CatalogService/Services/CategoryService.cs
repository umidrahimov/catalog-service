using System;
using System.Data;
using AutoMapper;
using CatalogService.Contracts;
using CatalogService.Data;
using CatalogService.Interfaces;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;

    public CategoryService(AppDbContext context, ILogger<CategoryService> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();

            return categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving Categories.");
            throw;
        }
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);

            return category;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"No Category with id {id} is found.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the Category with id {id}.");
            throw;
        }
    }

    public async Task CreateCategoryAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = _mapper.Map<Category>(request);

            if (_context.Categories.Any(x => x.Name.Equals(category.Name)))
            {
                throw new DuplicateNameException($"Category with name {category.Name} already exists");
            }

            category.CreatedAt = DateTime.UtcNow;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        catch (DuplicateNameException ex)
        {
            _logger.LogError(ex, $"Category with name {request.Name} already exists");
            throw new DuplicateNameException($"Category with name {request.Name} already exists");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the Category.");
            throw new Exception("An error occurred while creating the Category.");
        }
    }

    public async Task UpdateCategoryAsync(int id, UpdateCategoryRequest request)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException($"No Category with id {id} is found.");
            }

            if (request.Name != null)
            {
                category.Name = request.Name;
            }

            if (request.Description != null)
            {
                category.Description = request.Description;
            }

            category.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"Category with id {id} not found.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the Category with id {id}.");
            throw;
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No Category found with the id {id}");
            }
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"No Category found with the id {id}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the Category with id {id}.");
            throw;
        }
    }
}
