public interface ITexturePatternRepository {
    Task<TexturePattern> GetByIdAsync(int id);
    Task<TexturePattern> GetByNameAsync(string name);
    Task<IEnumerable<TexturePattern>> GetAllAsync();
    Task<TexturePattern> AddAsync(TexturePattern TexturePattern);
    Task DeleteAsync(int id);
    Task<IEnumerable<TexturePattern>> GetByIdsAsync(IEnumerable<int> ids);
}