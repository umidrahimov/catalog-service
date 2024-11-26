using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Contracts;

public class UpdateItemRequest
{
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(500)]
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}
