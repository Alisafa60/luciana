public interface IFabricRepository {
    Task<Fabric> GetByIdAsync(int id);
    Task<Fabric> GetByNameAsync(string name);
    Task<IEnumerable<Fabric>> GetAllAsync();
    Task<Fabric> AddAsync(Fabric fabric);
    Task DeleteAsync(int id);
    Task<IEnumerable<Fabric>> GetByIdsAsync(IEnumerable<int> ids);
}