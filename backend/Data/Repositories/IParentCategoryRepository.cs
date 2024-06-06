public interface IParentCategoryRepository {
    Task<ParentCategory> GetByIdAsync(int id);
    Task<ParentCategory> GetByNameAsync(string name);
    Task<IEnumerable<ParentCategory>> GetAllAsync();
    Task<ParentCategory> AddAsync(ParentCategory parentCategory);
    Task DeleteAsync(int id);
}