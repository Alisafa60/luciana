#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

public class Product{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
    
    [Required]
    public decimal Price { get; set ;}

    public int? Stock { get; set; }

    public string? ForChildren { get; set; }

    public decimal? Weight { get; set; }

    public int ProductSizeId { get; set; }

    public string ProductPicturePath { get; set; }

    [ForeignKey(nameof(ProductSizeId))]
    public Size Size { get; set; }

    public virtual ICollection<ProductTexturePattern> ProductTexturePatterns {get; set;}
    public virtual ICollection<ProductColor> ProductColors { get; set; }
    public virtual ICollection<ProductFabric> ProductFabrics{ get; set;}
    public virtual ICollection<ProductCategory> ProductCategories{ get; set;}
    public virtual ICollection<ProductPromotion> ProductPromotions { get; set; }
    public virtual ICollection<ProductTag> ProductTags { get; set; }
}

