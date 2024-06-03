using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class ProductModel {
    public int Id { get; set; }
    [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    public int? Stock { get; set; }

    public bool ForChildren { get; set; }

    public decimal Weight { get; set; }

    public int ProductSizeId { get; set; }

    public ICollection<int> ProductTexturePatternIds { get; set; } = new List<int>();
    public ICollection<int> ProductColorIds { get; set; } = new List<int>();
    public ICollection<int> ProductFabricIds { get; set; } = new List<int>();
    public ICollection<int> ProductCategoryIds { get; set; } = new List<int>();
    public ICollection<int> ProductPromotionIds { get; set; } = new List<int>();
    
    public IFormFile Picture { get; set; }
}
