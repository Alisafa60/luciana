using backend.Data;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository {
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Cart> GetByIdAsync(int id) {
        return await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cart> AddAsync(Cart cart) {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart) {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task DeleteAsync(int id) {
        var cart = await _context.Carts.FindAsync(id);
        if (cart != null) {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }
    }
}