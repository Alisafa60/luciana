using backend.Data;
using Microsoft.EntityFrameworkCore;

public class FabricRepository : IFabricRepository {
    private readonly AppDbContext _context;

    public FabricRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Fabric> GetByIdAsync(int id) {
        return await _context.Fabrics.FindAsync(id);
    }

    public async Task<Fabric> GetByNameAsync(string name) {
        return await _context.Fabrics.FirstOrDefaultAsync(f => f.Name == name);
    }

    public async Task<IEnumerable<Fabric>> GetAllAsync() {
        return await _context.Fabrics.ToListAsync();
    }

    public async Task<Fabric> AddAsync(Fabric fabric) {
        await _context.Fabrics.AddAsync(fabric);
        await _context.SaveChangesAsync();
        return fabric;
    }

    public async Task DeleteAsync(int id) {
        var fabric = await _context.Fabrics.FindAsync(id);
        if (fabric != null) {
            _context.Remove(fabric);
            await _context.SaveChangesAsync();
        }
    }
}