public interface IProductRepository {
    Task<Product> GetByIdAsync(int id);
    Task<Product> GetByNameAsync(string name);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> AddAsync(Product Product);
    Task DeleteAsync(int id);
}