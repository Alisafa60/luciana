using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;


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
        var parentFabric = new ParentFabric{
            Name = parentFabricModel.Name,
        };

        await _context.ParentFabrics.AddAsync(parentFabric);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParentFabric), new {id = parentFabric.Id}, parentFabricModel);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentFabricModel>>> GetParentFabrics() {
        var parentFabrics = await _context.ParentFabrics
            .Select( pf => new ParentFabricModel {Name = pf.Name,}).ToListAsync();

            return parentFabrics;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentFabricModel>> GetParentFabric(int id) {
        var parentFabric = await _context.ParentFabrics.FindAsync(id);
        if (parentFabric == null) {
            return NotFound();
        }

        var parentFabricModel = new ParentFabricModel{
            Id = parentFabric.Id,
            Name = parentFabric.Name,
        };

        return parentFabricModel;
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentFabricModel>> GetParentFabricByName(string name) { 
        var parentFabric = await _context.ParentFabrics
            .FirstOrDefaultAsync( pf => pf.Name == name);

        if (parentFabric == null) {
            return NotFound();
        }

        var parentFabricModel = new ParentFabricModel {
                Id = parentFabric.Id,
                Name= parentFabric.Name,
        };

        return Ok(parentFabricModel);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteParentFabric(int id) {
        var parentFabric = await _context.ParentFabrics.FindAsync(id);
        if (parentFabric == null) { 
            return NotFound(); 
        }

        _context.ParentFabrics.Remove(parentFabric);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

