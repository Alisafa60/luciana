#pragma warning disable CS8603 // Possible null reference return.
using backend.Data;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) {
        _context = context;
    }
    public async Task<Category> GetByIdAsync(int id) {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync() {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetByNameAsync(string name) {
        return await _context.Categories
            .Where(c => c.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<Category> AddAsync(Category category) {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(int id) {
        var category = await GetByIdAsync(id);
        if (category != null) {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<int> ids){
        return await _context.Categories.Where(c => ids.Contains(c.Id)).ToListAsync();
    }
}

#pragma warning restore CS8603 // Possible null reference return