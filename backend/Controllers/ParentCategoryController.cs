using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using System.Threading.Tasks;

[Route("api/parentcategory")]
[ApiController]
public class ParentCategoryController : ControllerBase {
    private readonly AppDbContext _context;

    public ParentCategoryController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ParentCategory>> CreateParentCategory(ParentCategoryModel categoryModel) {
        var category = new ParentCategory{Name = categoryModel.Name};
        await _context.ParentCategories.AddAsync(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParentCategory), new { id = category.Id }, category);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentCategory>>> GetParentCategories() {
        var categories = await _context.ParentCategories
            .Select(pc => new ParentCategory{
                Id = pc.Id,
                Name = pc.Name,
            })
            .ToListAsync();

        return categories;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentCategoryModel>> GetParentCategory(int id) {
        var category = await _context.ParentCategories.FindAsync(id);
        if (category == null) {
            return NotFound();
        }

        var categoryModel = new ParentCategoryModel {
            Id = category.Id,
            Name = category.Name,
        };

        return categoryModel;
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentCategoryModel>> GetParentCategoryByName(string name) {
        var category = await _context.ParentCategories.FirstOrDefaultAsync(pc => pc.Name == name);
        if (category == null) {
            return NotFound();
        }

        var categoryModel = new ParentCategoryModel {
            Id = category.Id,
            Name = category.Name
        };

        return Ok(categoryModel);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteParentCategory(int id) {
        var category = await _context.ParentCategories.FindAsync(id);
        if (category == null) {
            return NotFound();
        }

        _context.ParentCategories.Remove(category);
        _context.SaveChanges();

        return NoContent();
    }
}
