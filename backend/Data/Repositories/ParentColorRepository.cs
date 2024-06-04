using backend.Data;
using Microsoft.EntityFrameworkCore;

public class ParentColorRepository : IParentColorRepository {
    private readonly AppDbContext _context;

    public ParentColorRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<ParentColor> GetByIdAsync(int id) {
        return await _context.ParentColors.FindAsync(id);
    }

    public async Task<ParentColor> GetByNameAsync(string name) {
        return await _context.ParentColors
            .Where( pc => pc.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ParentColor>> GetAllAsync() {
        return await _context.ParentColors.ToListAsync();
    }

    public async Task<ParentColor> AddAsync(ParentColor parentColor) {
        await _context.ParentColors.AddAsync(parentColor);
        await _context.SaveChangesAsync();
        return parentColor;
    }

    public async Task DeleteAsync(int id) {
        var parentColor = await _context.ParentColors.FindAsync(id);
        if (parentColor != null) {
            _context.ParentColors.Remove(parentColor);
            await _context.SaveChangesAsync();
        }
    }
}