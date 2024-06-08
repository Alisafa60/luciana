public interface IProductHistoryRepository {
    Task<IEnumerable<ProductHistory>> GetAllAsync();
    Task<ProductHistory> GetByIdAsync(int id);
}