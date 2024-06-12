using Microsoft.EntityFrameworkCore;

public class Cart {
    public int Id { get; set; }

    public string SessionId { get; set; }

    public DateTime CreatedDate { get; set;} = DateTime.UtcNow;
    
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}