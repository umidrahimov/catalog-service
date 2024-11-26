using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Contracts;

public class CreateItemRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    public decimal Price { get; set; }
    [Required]
    public int CategoryId { get; set; }
}
