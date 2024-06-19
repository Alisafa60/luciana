using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase {
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public CartController(ICartRepository cartRepository, ICartItemRepository cartItemRepository) {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    [HttpGet("{cartId}")]
    public async Task<ActionResult<CartDto>> GetCartById(int cartId) {
        var cart = await _cartRepository.GetByIdAsync(cartId);
        if (cart == null) {
            return NotFound();
        }

        var cartDto = MapToCartDto(cart);
        return Ok(cartDto);
    }

    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart() {
        var sessionId = HttpContext.Session.GetOrCreateSessionId();
        var cart = await _cartRepository.GetBySessionIdAsync(sessionId);
        if (cart == null) {
            return NotFound();
        }

        var cartDto = MapToCartDto(cart);
        return Ok(cartDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<CartDto>> AddCart(CartDto cartDto) {
        try {
            var sessionId = HttpContext.Session.GetOrCreateSessionId();
            var cart = MapToCart(cartDto);
            cart.SessionId = sessionId;
            var createdCart = await _cartRepository.AddAsync(cart);
            var createdCartDto = MapToCartDto(createdCart);
            return CreatedAtAction(nameof(GetCartById), new { cartId = createdCartDto.Id}, createdCartDto);
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

    [HttpPost("{cartId}/items")]
    public async Task<ActionResult<CartItemDto>> AddCartItem(int cartId, CartItemDto cartItemDto) {
        try {
            var cartItem = MapToCartItem(cartItemDto);
            cartItem.CartId = cartId;
            var createdCartItem = await _cartItemRepository.AddAsync(cartItem);
            var createdCartItemDto = MapToCartItemDto(createdCartItem);

            return CreatedAtAction(nameof(GetCartById), new { cartId }, createdCartItemDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpPut("items/{cartItemId}")]
    public async Task <ActionResult<CartItemDto>> UpdateCartItem(int cartItemId, CartItemDto cartItemDto) {
       try {
           if (cartItemId != cartItemDto.Id) {
                return BadRequest();
            }

            var cartItem = MapToCartItem(cartItemDto);
            var updatedCartItem = await _cartItemRepository.UpdateAsync(cartItem);
            var updatedCartItemDto = MapToCartItemDto(updatedCartItem);

            return Ok(updatedCartItemDto);
       } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> DeleteCartItem(int cartItemId) {
        try {
            await _cartItemRepository.DeleteAsync(cartItemId);
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