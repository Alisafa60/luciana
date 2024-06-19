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
    public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto categoryDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var category = MapToCategory(categoryDto);
            await _categoryRepository.AddAsync(category);
            var createdCategoryDto = MapToCategoryDto(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, createdCategoryDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories() {
        try {
            var categories = await _categoryRepository.GetAllAsync();

            return Ok(categories);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id) {
        try {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) {
                return NotFound();
            }

            var categoryDto = MapToCategoryDto(category);

            return Ok(categoryDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryByName(string name) {
        try {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null) {
                return NotFound();
            }

            var categoryDto = MapToCategoryDto(category);

            return Ok(categoryDto);
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

    private Category MapToCategory(CategoryDto categoryDto) {
        return new Category {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCategoryId,
        };
    
    }

    private CategoryDto MapToCategoryDto (Category category) {
        return new CategoryDto {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
        };
    }
}
