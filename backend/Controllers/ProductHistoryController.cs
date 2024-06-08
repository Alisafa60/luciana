using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/producthistory")]
[ApiController]
public class ProductHistoryController : ControllerBase{
    private readonly IProductHistoryRepository _historyRepository;

    public ProductHistoryController(IProductHistoryRepository historyRepository) {
        _historyRepository = historyRepository;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ProductHistoryDto>>> GetProductHistories() {
        var productHistories = await _historyRepository.GetAllAsync();
        return Ok(productHistories);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductHistoryDto>> GetProductHistory(int id) {
        var productHistory = await _historyRepository.GetByIdAsync(id);
        if (productHistory == null) {
            return NotFound();
        }

        var productHistoryDto = new ProductHistoryDto {
            Id = productHistory.Id,
            ProductId = productHistory.ProductId,
            ProductName = productHistory.ProductName,
            ProductDescription = productHistory.ProductDescription,
            ChangeDate = productHistory.ChangeDate,
        };

        return Ok(productHistoryDto);
    }
}