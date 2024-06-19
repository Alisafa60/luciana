using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using System.Threading.Tasks;

[Route("api/parentfabric")]
[ApiController]
public class ParentFabricController : ControllerBase {
    private readonly IParentFabricRepository _parentFabricRepository;
    
    public ParentFabricController(IParentFabricRepository parentFabricRepository) {
        _parentFabricRepository = parentFabricRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ParentFabricDto>> CreateParentFabric(ParentFabricDto parentFabricDto) {
        try {
            var parentFabric = MapToParentFabric(parentFabricDto);
            await _parentFabricRepository.AddAsync(parentFabric);
            var createdParentFabricDto = MapToParentFabricDto(parentFabric);

            return CreatedAtAction(nameof(GetParentFabric), new { id = parentFabric.Id }, createdParentFabricDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentFabricDto>>> GetParentFabrics() {
        try {
            var parentFabrics = await _parentFabricRepository.GetAllAsync();

            return Ok(parentFabrics);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentFabricDto>> GetParentFabric(int id) {
        try {
            var parentFabric = await _parentFabricRepository.GetByIdAsync(id);
            if (parentFabric == null) {
                return NotFound();
            }

            var parentFabricDto = MapToParentFabricDto(parentFabric);

            return Ok(parentFabricDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentFabricDto>> GetParentFabricByName(string name) {
        try {
            var parentFabric = await _parentFabricRepository.GetByNameAsync(name);
            if (parentFabric == null) {
                return NotFound();
            }

            var parentFabricDto = MapToParentFabricDto(parentFabric);

            return Ok(parentFabricDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteParentFabric(int id) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            await _parentFabricRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private ParentFabric MapToParentFabric(ParentFabricDto parentFabricDto) {
        return new ParentFabric {
            Id = parentFabricDto.Id,
            Name = parentFabricDto.Name,
        };
    }

    private ParentFabricDto MapToParentFabricDto(ParentFabric parentFabric) {
        return new ParentFabricDto {
            Id = parentFabric.Id,
            Name = parentFabric.Name,
        };
    }
}
