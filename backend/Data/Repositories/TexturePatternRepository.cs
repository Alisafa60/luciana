using backend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8603 // Possible null reference return.

public class TexturePatternRepository : ITexturePatternRepository {
    private readonly AppDbContext _context;

    public TexturePatternRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<TexturePattern> GetByIdAsync(int id) {

        return await _context.TexturePatterns.FindAsync(id);

    }

    public async Task<IEnumerable<TexturePattern>> GetAllAsync() {
        return await _context.TexturePatterns.ToListAsync();
    }

    public async Task<TexturePattern> GetByNameAsync(string name) {
        return await _context.TexturePatterns.FirstOrDefaultAsync(tp => tp.Name == name);
    }

    public async Task<TexturePattern> AddAsync(TexturePattern texturePattern) {
        await _context.TexturePatterns.AddAsync(texturePattern);
        await _context.SaveChangesAsync();
        return texturePattern;
    }

    public async Task DeleteAsync(int id) {
        var texturePattern = await _context.TexturePatterns.FindAsync(id);
        if (texturePattern != null) {
            _context.TexturePatterns.Remove(texturePattern);
            await _context.SaveChangesAsync();
        }
    }
}

#pragma warning restore CS8603 // Possible null reference return.