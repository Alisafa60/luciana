using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/TexturePattern")]
[ApiController]
public class TexturePatternController : ControllerBase {
    private readonly AppDbContext _context;
    public TexturePatternController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TexturePatternModel>> CreateTexturePattern (TexturePatternModel texturePatternModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var texturePattern = new TexturePattern {
                Name = texturePatternModel.Name,
            };

            await _context.TexturePatterns.AddAsync(texturePattern);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTexturePattern), new {id = texturePatternModel.Id});
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TexturePatternModel>>> GetTexturePatterns () {
        try {
            var texturePatterns = await _context.TexturePatterns
                .Select(x => new TexturePatternModel {
                    Name = x.Name,
                    Id = x.Id,
                })
                .ToListAsync();

            return Ok(texturePatterns);
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TexturePatternModel>> GetTexturePattern (int id) {
        try {
            var texturePattern = await _context.TexturePatterns.FindAsync(id);
            if (texturePattern == null) {
                return NotFound();
            }

            var texturePatternModel = new TexturePatternModel {
                Name = texturePattern.Name,
                Id = texturePattern.Id,
            };

            return Ok(texturePatternModel);
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTexturePattern (int id) {
        var texturePattern = await _context.TexturePatterns.FindAsync(id);
        if(texturePattern == null) {
            return BadRequest();
        }

        _context.TexturePatterns.Remove(texturePattern);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
