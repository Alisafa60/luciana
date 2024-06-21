public class CartDto {
    public int Id { get; set; }
    public ICollection<CartItemDto> CartItems { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public decimal TotalPrice { get; set; }
}