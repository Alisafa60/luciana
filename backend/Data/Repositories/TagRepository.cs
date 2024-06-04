using backend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8603 // Possible null reference return.

public class TagRepository : ITagRepository {
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Tag> GetByIdAsync(int id) {

        return await _context.Tags.FindAsync(id);

    }

    public async Task<IEnumerable<Tag>> GetAllAsync() {
        return await _context.Tags.ToListAsync();
    }

    public async Task<Tag> AddAsync(Tag tag) {
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public async Task DeleteAsync(int id) {
        var tag = await _context.Tags.FindAsync(id);
        if (tag != null) {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}

#pragma warning restore CS8603 // Possible null reference return.