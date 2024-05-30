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
    private readonly AppDbContext _context;
    
    public ParentFabricController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ParentFabricModel>> CreateParentFabric(ParentFabricModel parentFabricModel) {
        try {
            var parentFabric = new ParentFabric {
                Name = parentFabricModel.Name,
            };

            await _context.ParentFabrics.AddAsync(parentFabric);
            await _context.SaveChangesAsync();

            var createdParentFabricModel = new ParentFabricModel {
                Id = parentFabric.Id, 
                Name = parentFabric.Name,
            };

            return CreatedAtAction(nameof(GetParentFabric), new { id = parentFabric.Id }, createdParentFabricModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentFabricModel>>> GetParentFabrics() {
        try {
            var parentFabrics = await _context.ParentFabrics
                .Select(pf => new ParentFabricModel { Name = pf.Name })
                .ToListAsync();

            return parentFabrics;
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentFabricModel>> GetParentFabric(int id) {
        try {
            var parentFabric = await _context.ParentFabrics.FindAsync(id);
            if (parentFabric == null) {
                return NotFound();
            }

            var parentFabricModel = new ParentFabricModel {
                Id = parentFabric.Id,
                Name = parentFabric.Name,
            };

            return parentFabricModel;
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentFabricModel>> GetParentFabricByName(string name) {
        try {
            var parentFabric = await _context.ParentFabrics.FirstOrDefaultAsync(pf => pf.Name == name);
            if (parentFabric == null) {
                return NotFound();
            }

            var parentFabricModel = new ParentFabricModel {
                Id = parentFabric.Id,
                Name = parentFabric.Name,
            };

            return Ok(parentFabricModel);
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
            var parentFabric = await _context.ParentFabrics.FindAsync(id);
            if (parentFabric == null) {
                return NotFound();
            }

            _context.ParentFabrics.Remove(parentFabric);
            await _context.SaveChangesAsync();

            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
