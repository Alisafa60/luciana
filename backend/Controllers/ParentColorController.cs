using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/parentcolor")]
[ApiController]
public class ParentColorController : ControllerBase {
    private readonly AppDbContext _context;

    public ParentColorController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ParentColorModel>> CreateParentColor(ParentColorModel parentColorModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var parentColor = new ParentColor {
                Name = parentColorModel.Name,
            };

            await _context.ParentColors.AddAsync(parentColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParentColor), new { id = parentColor.Id }, parentColor);
        } catch (Exception ex){
            return StatusCode(500, $"Internal server error {ex.Message}");
       }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentColorModel>>> GetParentColors() {
       try{
            var parentColors = await _context.ParentColors
                .Select(pc => new ParentColorModel {
                    Id = pc.Id,
                    Name = pc.Name,
                })
                .ToListAsync();

            return Ok(parentColors);
       } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentColorModel>> GetParentColor(int id) {
        try {
            var parentColor = await _context.ParentColors.FindAsync(id);
            if (parentColor == null) {
                return NotFound();
            }

            var parentColorModel = new ParentColorModel {
                Id = parentColor.Id,
                Name = parentColor.Name,
            };

            return Ok(parentColorModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }
}