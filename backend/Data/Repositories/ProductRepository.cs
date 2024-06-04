using backend.Data;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository {
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Product> GetByIdAsync(int id) {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> GetByNameAsync(string name) {
        return await _context.Products
            .Where(p => p.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync() {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> AddAsync(Product product) {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task DeleteAsync(int id) {
        var product = await _context.Products.FindAsync(id);
        if (product != null) {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}