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
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemsByCategoryIdAsync([FromQuery] int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var items = await _itemService.GetItemsByCategoryAsync(categoryId, page, pageSize);
                if (items == null || !items.Any())
                {
                    return Ok(new { message = $"No Items found.", data = new List<Item>() });
                }
                return Ok(new { message = $"Successfully retrieved the Items.", data = items });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the Items", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                if (item == null)
                {
                    return NotFound(new { message = $"No Item with Id {id} found." });
                }
                return Ok(new { message = $"Successfully retrieved Item with Id {id}.", data = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving Item with id {id}", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync(CreateItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _itemService.CreateItemAsync(request);
                return Ok(new { message = "The Item successfully created" });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = "An error occurred while creating the Item.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the Item", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, UpdateItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                if (item == null)
                {
                    return NotFound(new { message = $"Item with id {id} not found" });
                }
                await _itemService.UpdateItemAsync(id, request);
                return Ok(new { message = $"Item with id {id} successfully updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while updating Item with id {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                if (item == null)
                {
                    return NotFound(new { message = $"Item with id {id} not found" });
                }
                await _itemService.DeleteItemAsync(id);
                return Ok(new { message = $"Item with id {id} successfully deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while deleting Item with id {id}", error = ex.Message });
            }
        }
    }
}
