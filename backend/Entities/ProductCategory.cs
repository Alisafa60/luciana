#pragma warning disable CS8618 

public class ProductCategory{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int CategoryId { get; set;}
    public Category Category { get; set; }
}