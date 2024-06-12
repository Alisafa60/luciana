using System.ComponentModel.DataAnnotations.Schema;

public class CartItem {
    public int Id { get; set; }

    public int CartId { get; set; }
    [ForeignKey("CartId")]
    public virtual Cart Cart { get; set; }

    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }

    public int Quantity  { get; set; }

    public decimal Price { get; set; }
}