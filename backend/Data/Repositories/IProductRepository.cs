public interface IProductRepository {
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> GetByNameAsync(string name);
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<Product> AddAsync(Product Product);
    Task DeleteAsync(int id);
}