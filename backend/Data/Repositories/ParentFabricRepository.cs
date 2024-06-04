using backend.Data;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8603 // Possible null reference return.
public class ParentFabricRepository : IParentFabricRepository {
    private readonly AppDbContext _context;
    public ParentFabricRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<ParentFabric> GetByIdAsync(int id) {
        return await _context.ParentFabrics.FindAsync(id);
    }

    public async Task<IEnumerable<ParentFabric>> GetAllAsync() {
        return await _context.ParentFabrics.ToListAsync();
    }

    public async Task<ParentFabric> GetByNameAsync(string name) {
        return await _context.ParentFabrics
            .Where(pf => pf.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<ParentFabric> AddAsync(ParentFabric parentFabric) {
        await _context.ParentFabrics.AddAsync(parentFabric);
        await _context.SaveChangesAsync();

        return parentFabric;
    }

    public async Task DeleteAsync(int id) {
        var parentFabric = await _context.ParentFabrics.FindAsync(id);
        if (parentFabric != null) {
            _context.ParentFabrics.Remove(parentFabric);
            await _context.SaveChangesAsync();
        }
    }
}