using System.Runtime.CompilerServices;
using backend.Data;
using Microsoft.EntityFrameworkCore;

public class ColorRepository : IColorRepository {
    private readonly AppDbContext _context;
    public ColorRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Color> GetByIdAsync(int id) {
        return await _context.Colors.FindAsync(id);
    }

    public async Task<IEnumerable<Color>> GetAllAsync() {
        return await _context.Colors.ToListAsync();
    }

    public async Task<Color> GetByNameAsync(string name) {
        return await _context.Colors
            .Where(c => c.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<Color> AddAsync(Color color) {
        await _context.Colors.AddAsync(color);
        await _context.SaveChangesAsync();
        return color;
    }

    public async Task DeleteAsync(int id) {
        var color = await _context.Colors.FindAsync(id);
        if (color != null) {
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Color>> GetByIdsAsync(IEnumerable<int> ids){
        return await _context.Colors.Where(c => ids.Contains(c.Id)).ToListAsync();
    }
}