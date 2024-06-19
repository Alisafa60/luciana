public interface ICartRepository {
    Task<Cart> GetByIdAsync(int id);
    Task<Cart> GetBySessionIdAsync(string sessionId);
    Task<Cart> AddAsync(Cart cart);
    Task<Cart> UpdateAsync(Cart cart);
    Task DeleteAsync(int id);
}