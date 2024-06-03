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
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<Category> AddAsync(Category category) {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(int id) {
        var category = await GetByIdAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
