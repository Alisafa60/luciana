public interface IProductRepository {
    Task<ProductModel> GetByIdAsync(int id);
    Task<ProductModel> GetByNameAsync(string name);
    Task<IEnumerable<ProductModel>> GetAllAsync();
    Task<Product> AddAsync(Product Product);
    Task DeleteAsync(int id);
}