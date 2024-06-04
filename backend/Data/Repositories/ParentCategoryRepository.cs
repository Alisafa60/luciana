using System.Runtime.Intrinsics.X86;
using backend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8603 // Possible null reference return.

public class ParentCategoryRepository : IParentCategoryRepository {
    private readonly AppDbContext _context;

    public ParentCategoryRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<ParentCategory> GetByIdAsync(int id) {

        return await _context.ParentCategories.FindAsync(id);

    }

    public async Task<IEnumerable<ParentCategory>> GetAllAsync() {
        return await _context.ParentCategories.ToListAsync();
    }

    public async Task<ParentCategory> GetByNameAsync(string name) {
        return await _context.ParentCategories
            .Where(pc => pc.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<ParentCategory> AddAsync(ParentCategory parentCategory) {
        await _context.ParentCategories.AddAsync(parentCategory);
        await _context.SaveChangesAsync();
        return parentCategory;
    }

    public async Task DeleteAsync(int id) {
        var parentCategory = await _context.ParentCategories.FindAsync(id);
        if (parentCategory != null) {
            _context.ParentCategories.Remove(parentCategory);
            await _context.SaveChangesAsync();
        }
    }
}

#pragma warning restore CS8603 // Possible null reference return.