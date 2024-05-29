using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<FabricModel> CreateFabric(FabricModel fabricModel) {
        try {
            var fabric = new Fabric {
                Name = fabricModel.Name,
                ParentFabricId = fabricModel.ParentFabricId,
            };

            _context.Fabrics.Add(fabric);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetFabric), new { id = fabric.Id }, fabricModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<FabricModel>> GetFabrics() {
        try {
            var fabrics = _context.Fabrics
                .Select(f => new FabricModel {
                    Id = f.Id,
                    Name = f.Name,
                    ParentFabricId = f.ParentFabricId,
                })
                .ToList();

            return fabrics;
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public ActionResult<FabricModel> GetFabric(int id) {
        try {
            var fabric = _context.Fabrics.Find(id);

            if (fabric == null) {
                return NotFound();
            }

            var fabricModel = new FabricModel {
                Id = fabric.Id,
                Name = fabric.Name,
                ParentFabricId = fabric.ParentFabricId,
            };

            return fabricModel;
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public ActionResult<FabricModel> GetFabricByName(string name) {
        try {
            var fabric = _context.Fabrics.FirstOrDefault(f => f.Name == name);

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
    public ActionResult DeleteFabric(int id) {
        try {
            var fabric = _context.Fabrics.Find(id);
            if (fabric == null) {
                return NotFound();
            }

            _context.Fabrics.Remove(fabric);
            _context.SaveChanges();

            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
