using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/color")]
[ApiController]
public class ColorController : ControllerBase {
    private readonly AppDbContext _context;
    public ColorController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ColorModel>> CreateColor(ColorModel colorModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var color = new Color{
                Name = colorModel.Name,
                ParentColorId = colorModel.ParentColorId,
            };

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColor), new { id = color.Id}, colorModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }   
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ColorModel>>> GetColors() {
        try {
             var colors = await _context.Colors
                .Select(c => new ColorModel {
                    Id = c.Id,
                    Name = c.Name,
                    ParentColorId = c.ParentColorId,
                })
                .ToListAsync();

            return Ok(colors);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
       
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColorModel>> GetColor(int id) {
        try {
            var color = await _context.Colors.FindAsync(id);
            if (color == null) {
                return NotFound();
            }

            var colorModel = new ColorModel {
                Id = color.Id,
                Name = color.Name,
                ParentColorId = color.ParentColorId
            };

            return Ok(colorModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ColorModel>> GetColorByName(string name) {
        try {
            var color = await _context.Colors.FirstOrDefaultAsync(c => c.Name == name);
            if (color == null) {
                return NotFound();
            }

            var colorModel = new ColorModel {
                Id = color.Id,
                Name = color.Name,
                ParentColorId = color.ParentColorId,
            };

            return Ok(colorModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteColor( int id) {
        var color = await _context.Colors.FindAsync(id);
        if (color == null) {
            return NotFound();
        }

        _context.Colors.Remove(color);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}