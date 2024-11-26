using System.Data;
using CatalogService.Contracts;
using CatalogService.Data;
using CatalogService.Interfaces;
using CatalogService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                if (categories == null || !categories.Any())
                {
                    return Ok(new { message = "No Categories found", data = new List<Category>() });
                }

                return Ok(new { message = "Successfully retrieved all categories", data = categories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving all Categories", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _categoryService.CreateCategoryAsync(request);
                return Ok(new { message = "Category successfully created" });
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(new { message = "An error occurred while creating the Category", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the Category", error = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = $"No Category with Id {id} found." });
                }

                return Ok(new { message = $"Successfully retrieved Category with Id {id}.", data = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving Category with id {id}", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = $"Category with id {id} not found" });
                }

                await _categoryService.UpdateCategoryAsync(id, request);
                return Ok(new { message = $" Category with id {id} successfully updated" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while updating Categorywith id {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = $"Category with id {id} not found" });
                }

                await _categoryService.DeleteCategoryAsync(id);
                return Ok(new { message = $"Category with id {id} successfully deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while deleting Category with id {id}", error = ex.Message });
            }
        }
    }
}
