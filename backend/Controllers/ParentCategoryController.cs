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
    private readonly IParentCategoryRepository _parentCategoryRepository;

    public ParentCategoryController(IParentCategoryRepository parentCategoryRepository) {
        _parentCategoryRepository = parentCategoryRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ParentCategoryDto>> CreateParentCategory(ParentCategoryDto categoryDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var category = new ParentCategory{Name = categoryDto.Name};
            await _parentCategoryRepository.AddAsync(category);

            var createdCategoryDto = new ParentCategoryDto {
                Name = category.Name,
                Id = category.Id,
            };

            return CreatedAtAction(nameof(GetParentCategory), new { id = category.Id }, category);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentCategoryDto>>> GetParentCategories() {
        try {
            var categories = await _parentCategoryRepository.GetAllAsync();

            return Ok(categories);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentCategoryDto>> GetParentCategory(int id) {
        try {
            var category = await _parentCategoryRepository.GetByIdAsync(id);
            if (category == null) {
                return NotFound();
            }

            var categoryDto = new ParentCategoryDto {
                Id = category.Id,
                Name = category.Name,
            };

            return Ok(categoryDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentCategoryDto>> GetParentCategoryByName(string name) {
        try {
            var category = await _parentCategoryRepository.GetByNameAsync(name);
            if (category == null) {
                return NotFound();
            }

            var categoryDto = new ParentCategoryDto {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParentCategory(int id) {
        try {
            await _parentCategoryRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
