#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations.Schema;

public class ProductTexturePattern{

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set;}

    public int TexturePatternId { get; set; }

    [ForeignKey(nameof(TexturePatternId))]

    public TexturePattern TexturePattern { get; set; }

}
