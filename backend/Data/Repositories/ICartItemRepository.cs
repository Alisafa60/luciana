public interface ICartItemRepository {
    Task<CartItem> AddAsync(CartItem item);
    Task<CartItem> UpdateAsync(CartItem item);
    Task DeleteAsync(int id);
}