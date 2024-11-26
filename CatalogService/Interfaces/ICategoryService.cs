using System;
using CatalogService.Contracts;
using CatalogService.Models;

namespace CatalogService.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task CreateCategoryAsync(CreateCategoryRequest request);
    Task UpdateCategoryAsync(int id, UpdateCategoryRequest request);
    Task DeleteCategoryAsync(int id);
}
