using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/Cart")]
[ApiController]
public class CartController : ControllerBase {
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public CartController(ICartRepository cartRepository, ICartItemRepository cartItemRepository) {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    [HttpGet("{cartId}")]
    public async Task<ActionResult<CartDto>> GetCartById (int cartId) {
        var cart = await _cartRepository.GetByIdAsync(cartId);
        if (cart == null) {
            return NotFound();
        }

        var cartDto = MapToCartDto(cart);
        return Ok(cartDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<CartDto>> AddCart(CartDto cartDto) {
        try {
            var cart = MapToCart(cartDto);
            var createdCart = await _cartRepository.AddAsync(cart);
            var createdCartDto = MapToCartDto(createdCart);
            return CreatedAtAction(nameof(GetCartById), new { id = createdCartDto.Id}, createdCartDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpPut("{cartId}")]
    public async Task<ActionResult<CartDto>> UpdateCart(int cartId, CartDto cartDto) {
        try {
            if (cartId != cartDto.Id) {
                return BadRequest();
            }

            var cart = MapToCart(cartDto);
            var updatedCart = await _cartRepository.UpdateAsync(cart);
            var updatedCartDto = MapToCartDto(updatedCart);
            return Ok(updatedCartDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpDelete("{cartId}")]
    public async Task<IActionResult> DeleteCart(int cartId) {
        try {
            await _cartRepository.DeleteAsync(cartId);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    private CartDto MapToCartDto(Cart cart) {
        return new CartDto {
            Id = cart.Id,
            CartItems = cart.CartItems.Select(MapToCartItemDto).ToList(),
            TotalPrice = cart.TotalPrice,
        };
    }

    private Cart MapToCart(CartDto cartDto) {
        return new Cart {
            Id = cartDto.Id,
            CartItems = cartDto.CartItems.Select(MapToCartItem).ToList(),
        };
    }

    private CartItemDto MapToCartItemDto(CartItem cartItem) {
        return new CartItemDto {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity,
            Price = cartItem.Price,
        };
    }

    private CartItem MapToCartItem(CartItemDto cartItemDto) {
        return new CartItem {
            Id = cartItemDto.Id,
            ProductId = cartItemDto.ProductId,
            Quantity = cartItemDto.Quantity,
            Price = cartItemDto.Price,
        };
    }
}