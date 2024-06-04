public interface IParentColorRepository {
    Task<ParentColor> GetByIdAsync(int id);
    Task<ParentColor> GetByNameAsync(string name);
    Task<IEnumerable<ParentColor>> GetAllAsync();
    Task<ParentColor> AddAsync(ParentColor ParentColor);
    Task DeleteAsync(int id);
}