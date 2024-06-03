using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using backend.Data;

public class ProductControllerTests
{
    private DbContextOptions<AppDbContext> _dbContextOptions;

    public ProductControllerTests() {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task CreateProduct_ShouldSaveFileAndReturnProduct()
    {
        // Arrange
        var mockFormFile = new Mock<IFormFile>();
        var content = "Test file content";
        var fileName = "test.png";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        mockFormFile.Setup(_ => _.OpenReadStream()).Returns(ms);
        mockFormFile.Setup(_ => _.FileName).Returns(fileName);
        mockFormFile.Setup(_ => _.Length).Returns(ms.Length);

        var productModel = new ProductModel
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.99m,
            Stock = 100,
            ForChildren = false,
            Weight = 1.5m,
            ProductSizeId = 1,
            Picture = mockFormFile.Object,
            ProductTexturePatternIds = new List<int> { 1 },
            ProductCategoryIds = new List<int> { 1 },
            ProductColorIds = new List<int> { 1 },
            ProductFabricIds = new List<int> { 1 },
            ProductPromotionIds = new List<int> { 1 }
        };

        using (var context = new AppDbContext(_dbContextOptions))
        {
            var controller = new ProductController(context);

            var result = await controller.CreateProduct(productModel);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<ProductModel>(createdAtActionResult.Value);
            Assert.Equal(productModel.Name, returnValue.Name);

            Assert.False(string.IsNullOrEmpty(returnValue.ProductPicturePath));
            Assert.True(File.Exists(returnValue.ProductPicturePath));

            if (File.Exists(returnValue.ProductPicturePath))
            {
                File.Delete(returnValue.ProductPicturePath);
            }
        }
    }
}
