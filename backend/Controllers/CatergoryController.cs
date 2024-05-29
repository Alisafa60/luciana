using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;

[Route("api/category")]
[ApiController]
public class CategoryController : ControllerBase {
    private readonly AppDbContext _context;
    
    public CategoryController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryModel>> CreateCategory(CategoryModel categoryModel) {
        var category = new Category{
            Name = categoryModel.Name,
            ParentCategoryId = categoryModel.ParentCategoryId
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new {id = category.Id}, categoryModel);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories() {
        var categories = await _context.Categories
            .Select( c => new CategoryModel {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId
            })
            .ToListAsync();

        return categories;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryModel>> GetCategory(int id) {
        var category = await _context.Categories.FindAsync(id);
        
        if (category == null){
            return NotFound();
        }

        var categoryModel = new CategoryModel {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId
        };

        return categoryModel;
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<CategoryModel>> GetCategoryByName(string name) {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        if (category == null) {
            return NotFound();
        }

        var categoryModel = new CategoryModel{
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId
        };

        return categoryModel;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCategory(int id){
        var category = await _context.Categories.FindAsync(id);
        if(category == null) {
            return NotFound("");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}