public class CartDto {
    public int Id { get; set; }
    public ICollection<CartItemDto> CartItems { get; set; }
    public decimal TotalPrice { get; set; }
}