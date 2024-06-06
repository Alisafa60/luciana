public interface ICategoryRepository {
    Task<Category> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByNameAsync(string name);
    Task<Category> AddAsync(Category category);
    Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<int> ids);
    Task DeleteAsync(int id);
}
