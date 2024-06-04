using backend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8603 // Possible null reference return.

public class SizeRepository : ISizeRepository {
    private readonly AppDbContext _context;

    public SizeRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Size> GetByIdAsync(int id) {

        return await _context.Sizes.FindAsync(id);

    }

    public async Task<IEnumerable<Size>> GetAllAsync() {
        return await _context.Sizes.ToListAsync();
    }

    public async Task<Size> AddAsync(Size size) {
        await _context.Sizes.AddAsync(size);
        await _context.SaveChangesAsync();
        return size;
    }

    public async Task DeleteAsync(int id) {
        var size = await _context.Sizes.FindAsync(id);
        if (size != null) {
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
        }
    }
}

#pragma warning restore CS8603 // Possible null reference return.