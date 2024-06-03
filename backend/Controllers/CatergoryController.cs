using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using System.Threading.Tasks;

[Route("api/category")]
[ApiController]
public class CategoryController : ControllerBase {
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository) {
        _categoryRepository = categoryRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryModel>> CreateCategory(CategoryModel categoryModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var category = new Category {
                Name = categoryModel.Name,
                ParentCategoryId = categoryModel.ParentCategoryId
            };

            await _categoryRepository.AddAsync(category);

            var createdCategoryModel = new CategoryModel {
                Name = category.Name,
                Id = category.Id,
                ParentCategoryId = category.ParentCategoryId
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, createdCategoryModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories() {
        try {
            var categories = await _categoryRepository.GetAllAsync();

            return Ok(categories);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryModel>> GetCategory(int id) {
        try {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null) {
                return NotFound();
            }

            var categoryModel = new CategoryModel {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };

            return Ok(categoryModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<CategoryModel>> GetCategoryByName(string name) {
        try {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null) {
                return NotFound();
            }

            var categoryModel = new CategoryModel {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };

            return Ok(categoryModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id) {
        try {
            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
