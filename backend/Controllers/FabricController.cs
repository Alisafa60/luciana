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
    private readonly AppDbContext _context;

    public FabricController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FabricModel>> CreateFabric(FabricModel fabricModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var fabric = new Fabric {
                Name = fabricModel.Name,
                ParentFabricId = fabricModel.ParentFabricId,
            };

            await _context.Fabrics.AddAsync(fabric);
            await _context.SaveChangesAsync();

            var createdFabricModel = new FabricModel {
                Name = fabric.Name,
                ParentFabricId = fabric.ParentFabricId,
                Id = fabric.Id,
            };

            return CreatedAtAction(nameof(GetFabric), new { id = fabric.Id }, createdFabricModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FabricModel>>> GetFabrics() {
        try {
            var fabrics = await _context.Fabrics
                .Select(f => new FabricModel {
                    Id = f.Id,
                    Name = f.Name,
                    ParentFabricId = f.ParentFabricId,
                })
                .ToListAsync();

            return Ok(fabrics);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FabricModel>> GetFabric(int id) {
        try {
            var fabric = await _context.Fabrics.FindAsync(id);

            if (fabric == null) {
                return NotFound();
            }

            var fabricModel = new FabricModel {
                Id = fabric.Id,
                Name = fabric.Name,
                ParentFabricId = fabric.ParentFabricId,
            };

            return Ok(fabricModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<FabricModel>> GetFabricByName(string name) {
        try {
            var fabric = await _context.Fabrics.FirstOrDefaultAsync(f => f.Name == name);

            if (fabric == null) {
                return NotFound();
            }

            var fabricModel = new FabricModel {
                Id = fabric.Id,
                Name = fabric.Name,
                ParentFabricId = fabric.ParentFabricId
            };

            return Ok(fabricModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFabric(int id) {
        try {
            var fabric = await _context.Fabrics.FindAsync(id);
            if (fabric == null) {
                return NotFound();
            }

            _context.Fabrics.Remove(fabric);
            await _context.SaveChangesAsync();

            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
