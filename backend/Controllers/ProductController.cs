using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;

[Route("api/product")]
[ApiController]
public class ProductController : ControllerBase {
    private readonly IProductRepository _productRepository;
    private readonly LuceneSearchService _searchService;

    public ProductController(IProductRepository productRepository, LuceneSearchService searchService) {
        _productRepository = productRepository;
        _searchService = searchService;
        
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        try {
            string picturePath = null;

            if (productDto.Picture != null && productDto.Picture.Length > 0) {
                picturePath = await SavePicture(productDto.Picture);
            }

            var product = new Product {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                ForChildren = productDto.ForChildren,
                Weight = productDto.Weight,
                ProductSizeId = productDto.ProductSizeId,
                ProductPicturePath = picturePath,
                ProductTexturePatterns = productDto.ProductTexturePatternIds.Select(id => new ProductTexturePattern { TexturePatternId = id }).ToList(),
                ProductCategories = productDto.ProductCategoryIds.Select(id => new ProductCategory { CategoryId = id }).ToList(),
                ProductColors = productDto.ProductColorIds.Select(id => new ProductColor { ColorId = id }).ToList(),
                ProductFabrics = productDto.ProductFabricIds.Select(id => new ProductFabric { FabricId = id }).ToList(),
                ProductPromotions = productDto.ProductPromotionIds.Select(id => new ProductPromotion { PromotionId = id }).ToList(),
                ProductTags = productDto.ProductTagIds.Select(id => new ProductTag { TagId = id }).ToList()
            };

            await _productRepository.AddAsync(product);
            
            productDto.Id = product.Id;
            await _searchService.AddOrUpdateProductToIndexAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts() {
        try {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id) {
        try {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ProductDto>> GetProductByName(string name) {
        try {
            var product = await _productRepository.GetByNameAsync(name);
            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id) {
        
        try {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("search")]
    public ActionResult<IEnumerable<ProductDto>> SearchProducts([FromBody] SearchRequest request) {
        try {
            var products = _searchService.SearchProducts(request.SearchTerm);
            return Ok(products);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private async Task<string> SavePicture(IFormFile pictureFile) {
        try {
            if (pictureFile != null && pictureFile.Length > 0) {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsDir)) {
                    Directory.CreateDirectory(uploadsDir);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(pictureFile.FileName)}";
                var filePath = Path.Combine(uploadsDir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    await pictureFile.CopyToAsync(fileStream);
                }
                return filePath;
            } else {
                return null;
            }
        } catch (Exception ex) {
            Console.WriteLine($"Error saving picture file: {ex.Message}");
            return null;
        }
    }
}
