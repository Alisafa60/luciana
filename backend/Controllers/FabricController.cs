using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/fabric")]
[ApiController]
public class FabricController : ControllerBase {
    private readonly IFabricRepository _fabricRepository;

    public FabricController(IFabricRepository fabricRepository) {
        _fabricRepository = fabricRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FabricDto>> CreateFabric(FabricDto fabricDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var fabric = new Fabric {
                Name = fabricDto.Name,
                ParentFabricId = fabricDto.ParentFabricId,
            };

            await _fabricRepository.AddAsync(fabric);

            var createdFabricDto = new FabricDto {
                Name = fabric.Name,
                ParentFabricId = fabric.ParentFabricId,
                Id = fabric.Id,
            };

            return CreatedAtAction(nameof(GetFabric), new { id = fabric.Id }, createdFabricDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FabricDto>>> GetFabrics() {
        try {
            var fabrics = await _fabricRepository.GetAllAsync();
            return Ok(fabrics);

        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FabricDto>> GetFabric(int id) {
        try {
            var fabric = await _fabricRepository.GetByIdAsync(id);

            if (fabric == null) {
                return NotFound();
            }

            var fabricDto = new FabricDto {
                Id = fabric.Id,
                Name = fabric.Name,
                ParentFabricId = fabric.ParentFabricId,
            };

            return Ok(fabricDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<FabricDto>> GetFabricByName(string name) {
        try {
            var fabric = await _fabricRepository.GetByNameAsync(name);

            if (fabric == null) {
                return NotFound();
            }

            var fabricDto = new FabricDto {
                Id = fabric.Id,
                Name = fabric.Name,
                ParentFabricId = fabric.ParentFabricId
            };

            return Ok(fabricDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFabric(int id) {
        try {
            await _fabricRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
