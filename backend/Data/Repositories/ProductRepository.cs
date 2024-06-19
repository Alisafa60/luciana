using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository {
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<ProductDto> GetByIdAsync(int id) {
        var product = await _context.Products
            .Include(p => p.ProductTexturePatterns)
                .ThenInclude(ptp => ptp.TexturePattern)
            .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductFabrics)
                .ThenInclude(pf => pf.Fabric)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductPromotions)
                .ThenInclude(pp => pp.Promotion)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) {
            return null;
        }

        return MapToProductDto(product);
    }

    public async Task<ProductDto> GetByNameAsync(string name) {
        var product = await _context.Products
            .Include(p => p.ProductTexturePatterns)
                .ThenInclude(ptp => ptp.TexturePattern)
            .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductFabrics)
                .ThenInclude(pf => pf.Fabric)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductPromotions)
                .ThenInclude(pp => pp.Promotion)
            .Where(p => p.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();

        if (product == null) {
            return null;
        }

        return MapToProductDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync() {
        var products = await _context.Products
            .Include(p => p.ProductTexturePatterns)
                .ThenInclude(ptp => ptp.TexturePattern)
            .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductFabrics)
                .ThenInclude(pf => pf.Fabric)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductPromotions)
                .ThenInclude(pp => pp.Promotion)
            .ToListAsync();

        return products.Select(MapToProductDto);
    }

    public async Task<Product> AddAsync(Product product) {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(int id) {
        var product = await _context.Products.FindAsync(id);
        if (product == null) {
            return;
        }

        var productHistory = new ProductHistory {
            ProductId = product.Id,
            ProductName = product.Name,
            ProductDescription = product.Description,
            ChangeDate = DateTime.Now.ToUniversalTime(),
        };
        _context.ProductHistories.Add(productHistory);
        _context.Products.Remove(product);

        await _context.SaveChangesAsync();
    }

    private ProductDto MapToProductDto(Product product) {
        return new ProductDto {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ForChildren = product.ForChildren,
            Weight = product.Weight,
            ProductSizeId = product.ProductSizeId,
            ProductTexturePatternIds = product.ProductTexturePatterns.Select(ptp => ptp.TexturePatternId).ToList(),
            ProductColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
            ProductFabricIds = product.ProductFabrics.Select(pf => pf.FabricId).ToList(),
            ProductCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList(),
            ProductPromotionIds = product.ProductPromotions.Select(pp => pp.PromotionId).ToList(),
        };
    }
}
