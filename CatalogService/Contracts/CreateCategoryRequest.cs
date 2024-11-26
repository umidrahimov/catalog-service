using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Contracts;

public class CreateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(500)]
    public string Description { get; set; }
}
