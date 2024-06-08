using backend.Data;
using Microsoft.EntityFrameworkCore;

public class ProductHistoryRepository : IProductHistoryRepository {
    private readonly AppDbContext _context;

    public ProductHistoryRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<ProductHistory>> GetAllAsync() {
        return await _context.ProductHistories.ToListAsync();
    }

    public async Task<ProductHistory> GetByIdAsync(int id) {
        return await _context.ProductHistories.FindAsync(id);
    }
}