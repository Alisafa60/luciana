using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class Cart {
    public int Id { get; set; }

    public string SessionId { get; set; }

    public DateTime CreatedDate { get; set;} = DateTime.UtcNow;
    
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [NotMapped]
    public decimal TotalPrice => CartItems.Sum( item => item.Price*item.Quantity );
}