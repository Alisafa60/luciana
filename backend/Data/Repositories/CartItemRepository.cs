using backend.Data;

public class CartItemRepository : ICartItemRepository {
    private readonly AppDbContext _context;

    public CartItemRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<CartItem> AddAsync(CartItem item) {
        await _context.CartItems.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<CartItem> UpdateAsync(CartItem item) {
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }   

    public async Task DeleteAsync(int id) {
        var item = await _context.CartItems.FindAsync(id);
        if (item != null) {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}