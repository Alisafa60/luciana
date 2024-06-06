public interface ITagRepository {
    Task<Tag> GetByIdAsync(int id);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<Tag> AddAsync(Tag Tag);
    Task DeleteAsync(int id);
    Task<IEnumerable<Tag>> GetByIdsAsync(IEnumerable<int> ids);
}