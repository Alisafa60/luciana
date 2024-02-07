#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

public class Product{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
    
    [Required]
    public decimal Price { get; set ;}

    public int? Stock { get; set; }

    public ICollection<ProductTexturePattern> ProductTexturePatterns {get; set;}
    public ICollection<ProductColor> ProductColors { get; set; }
}
