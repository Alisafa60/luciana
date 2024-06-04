public interface IParentFabricRepository {
    Task<ParentFabric> GetByIdAsync(int id);
    Task<ParentFabric> GetByNameAsync(string name);
    Task<IEnumerable<ParentFabric>> GetAllAsync();
    Task<ParentFabric> AddAsync(ParentFabric parentFabric);
    Task DeleteAsync(int id);
}