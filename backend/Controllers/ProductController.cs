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
    public async Task<ActionResult<ProductModel>> CreateProduct(ProductModel productModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        try {
            string picturePath = null;

            if (productModel.Picture != null && productModel.Picture.Length > 0) {
                picturePath = await SavePicture(productModel.Picture);
            }

            var product = new Product {
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                Stock = productModel.Stock,
                ForChildren = productModel.ForChildren,
                Weight = productModel.Weight,
                ProductSizeId = productModel.ProductSizeId,
                ProductPicturePath = picturePath,
                ProductTexturePatterns = productModel.ProductTexturePatternIds.Select(id => new ProductTexturePattern { TexturePatternId = id }).ToList(),
                ProductCategories = productModel.ProductCategoryIds.Select(id => new ProductCategory { CategoryId = id }).ToList(),
                ProductColors = productModel.ProductColorIds.Select(id => new ProductColor { ColorId = id }).ToList(),
                ProductFabrics = productModel.ProductFabricIds.Select(id => new ProductFabric { FabricId = id }).ToList(),
                ProductPromotions = productModel.ProductPromotionIds.Select(id => new ProductPromotion { PromotionId = id }).ToList(),
                ProductTags = productModel.ProductTagIds.Select(id => new ProductTag { TagId = id }).ToList()
            };

            await _productRepository.AddAsync(product);
            
            productModel.Id = product.Id;
            await _searchService.AddOrUpdateProductToIndexAsync(productModel);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts() {
        try {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> GetProduct(int id) {
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
    public async Task<ActionResult<ProductModel>> GetProductByName(string name) {
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
    public ActionResult<IEnumerable<ProductModel>> SearchProducts([FromBody] SearchRequest request) {
        try {
            var products = _searchService.SearchProducts(request.SearchTerm, request.FuzzySearch);
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
