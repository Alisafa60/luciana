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

    public async Task<IEnumerable<ProductDto>> GetAllAsync() {
        return await _context.Products
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
            .Select(p => new ProductDto {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ForChildren = p.ForChildren,
                Weight = p.Weight,
                ProductSizeId = p.ProductSizeId,
                ProductTexturePatternIds = p.ProductTexturePatterns.Select(ptp => ptp.TexturePatternId).ToList(),
                ProductColorIds = p.ProductColors.Select(pc => pc.ColorId).ToList(),
                ProductFabricIds = p.ProductFabrics.Select(pf => pf.FabricId).ToList(),
                ProductCategoryIds = p.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                ProductPromotionIds = p.ProductPromotions.Select(pp => pp.PromotionId).ToList(),
            })
            .ToListAsync();
    }

    public async Task<Product> AddAsync(Product product) {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(int id) {
        var product = await _context.Products.FindAsync(id);
        if (product != null) {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
