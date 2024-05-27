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
    public ActionResult<ParentFabricModel> CreateParentFabric(ParentFabricModel parentFabricModel) {
        var parentFabric = new ParentFabric{
            Name = parentFabricModel.Name,
        };

        _context.ParentFabrics.Add(parentFabric);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetParentFabric), new {id = parentFabric.Id}, parentFabricModel);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ParentFabricModel>> GetParentFabrics() {
        var parentFabrics = _context.ParentFabrics
            .Select ( pf => new ParentFabricModel {Name = pf.Name,}).ToList();

            return parentFabrics;
    }

    [HttpGet("{id}")]
    public ActionResult<ParentFabricModel> GetParentFabric(int id) {
        var parentFabric = _context.ParentFabrics.Find(id);
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
    public ActionResult<ParentFabricModel> GetParentFabricByName(String name) { 
        var parentFabric = _context.ParentFabrics
            .FirstOrDefault( pf => pf.Name == name);

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
    public IActionResult DeleteParentFabric(int id) {
        var parentFabric = _context.ParentFabrics.Find(id);
        if (parentFabric == null) { 
            return NotFound(); 
        }

        _context.ParentFabrics.Remove(parentFabric);
        _context.SaveChanges();

        return NoContent();
    }
}

