using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var fabric = new Fabric{
            Name = fabricModel.Name,
            ParentFabricId = fabricModel.ParentFabricId,
        };

        _context.Fabrics.Add(fabric);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetFabric), new {id = fabric.Id}, fabricModel);
    }

    [HttpGet]
    public ActionResult<IEnumerable<FabricModel>> GetFabrics() {
        var fabrics = _context.Fabrics
            .Select( f => new FabricModel {
                Id = f.Id,
                Name = f.Name,
                ParentFabricId = f.ParentFabricId,
            })
            .ToList();

        return fabrics;
    }

    [HttpGet("{id}")]
    public ActionResult<FabricModel> GetFabric(int id) {
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
    }

    [HttpGet("name/{name}")]
    public ActionResult<FabricModel> GetFabricByName(string name) {
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
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteFabric(int id) {
        var fabric = _context.Fabrics.Find(id);
        if (fabric == null) {
            return NotFound();
        }

        _context.Fabrics.Remove(fabric);
        _context.SaveChanges();

        return NoContent();
    }
}