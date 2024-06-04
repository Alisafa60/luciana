public interface ISizeRepository {
    Task<Size> GetByIdAsync(int id);
    Task<IEnumerable<Size>> GetAllAsync();
    Task<Size> AddAsync(Size Size);
    Task DeleteAsync(int id);
}