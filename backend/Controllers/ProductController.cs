using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using backend.Migrations;

[Route("api/product")]
[ApiController]
public class ProductController : ControllerBase {
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ProductModel>> CreateProduct(ProductModel productModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var product = new Product {
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                Stock = productModel.Stock,
                ForChildren = productModel.ForChildren,
                Weight = productModel.Weight,
                ProductSizeId = productModel.ProductSizeId,
                ProductPicture = productModel.ProductPicture,
                ProductTexturePatterns = productModel.ProductTexturePatternIds.Select ( id => new ProductTexturePattern { TexturePatternId = id, }).ToList(),
                ProductCategories = productModel.ProductCategoryIds.Select( id => new ProductCategory { CategoryId = id,}).ToList(),
                ProductColors = productModel.ProductColorIds.Select( id => new ProductColor { ColorId = id, }).ToList(),
                ProductPromotions = productModel.ProductPromotionIds.Select( id => new ProductPromotion { PromotionId = id,}).ToList(),
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            productModel.Id = product.Id;

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productModel);
        }
        catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts() {
        try {
            var products = await _context.Products
                .Include(p => p.ProductTexturePatterns)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductFabrics)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductPromotions)
                .Select(p => new ProductModel {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ForChildren = p.ForChildren,
                    Weight = p.Weight,
                    ProductSizeId = p.ProductSizeId,
                    ProductPicture = p.ProductPicture,
                    ProductTexturePatternIds = p.ProductTexturePatterns.Select(ptp => ptp.TexturePatternId).ToList(),
                    ProductColorIds = p.ProductColors.Select(pc => pc.ColorId).ToList(),
                    ProductFabricIds = p.ProductFabrics.Select(pf => pf.FabricId).ToList(),
                    ProductCategoryIds = p.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                    ProductPromotionIds = p.ProductPromotions.Select(pp => pp.PromotionId).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }
        catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> GetProduct(int id) {
        try {
            var product = await _context.Products
                .Include(p => p.ProductTexturePatterns)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductFabrics)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductPromotions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) {
                return NotFound();
            }

            var productModel = new ProductModel {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ForChildren = product.ForChildren,
                Weight = product.Weight,
                ProductSizeId = product.ProductSizeId,
                ProductPicture = product.ProductPicture,
                ProductTexturePatternIds = product.ProductTexturePatterns.Select(ptp => ptp.TexturePatternId).ToList(),
                ProductColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
                ProductFabricIds = product.ProductFabrics.Select(pf => pf.FabricId).ToList(),
                ProductCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                ProductPromotionIds = product.ProductPromotions.Select(pp => pp.PromotionId).ToList(),
            };

            return Ok(productModel);
        }
        catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id) {
        var product = await _context.Products.FindAsync(id);

        if (product == null) {
            return NotFound();
        }

        try {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
