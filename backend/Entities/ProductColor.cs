#pragma warning disable CS8618 

public class ProductColor{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int ColorId { get; set;}
    public Color Color { get; set; }
}