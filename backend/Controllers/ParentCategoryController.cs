using Microsoft.AspNetCore.Mvc;
using System;
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
        try {
            var category = new ParentCategory{Name = categoryModel.Name};
            await _context.ParentCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParentCategory), new { id = category.Id }, category);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentCategory>>> GetParentCategories() {
        try {
            var categories = await _context.ParentCategories
                .Select(pc => new ParentCategory{
                    Id = pc.Id,
                    Name = pc.Name,
                })
                .ToListAsync();

            return categories;
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentCategoryModel>> GetParentCategory(int id) {
        try {
            var category = await _context.ParentCategories.FindAsync(id);
            if (category == null) {
                return NotFound();
            }

            var categoryModel = new ParentCategoryModel {
                Id = category.Id,
                Name = category.Name,
            };

            return categoryModel;
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentCategoryModel>> GetParentCategoryByName(string name) {
        try {
            var category = await _context.ParentCategories.FirstOrDefaultAsync(pc => pc.Name == name);
            if (category == null) {
                return NotFound();
            }

            var categoryModel = new ParentCategoryModel {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteParentCategory(int id) {
        try {
            var category = await _context.ParentCategories.FindAsync(id);
            if (category == null) {
                return NotFound();
            }

            _context.ParentCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
