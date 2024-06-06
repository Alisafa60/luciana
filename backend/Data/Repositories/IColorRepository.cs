public interface IColorRepository {
    Task<Color> GetByIdAsync(int id);
    Task<Color> GetByNameAsync(string name);
    Task<IEnumerable<Color>> GetAllAsync();
    Task<Color> AddAsync(Color PColor);
    Task<IEnumerable<Color>> GetByIdsAsync(IEnumerable<int> ids);
    Task DeleteAsync(int id);
}