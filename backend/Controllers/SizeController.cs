using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/size")]
[ApiController]
public class SizeController : ControllerBase {
    private readonly AppDbContext _context;
    public SizeController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SizeModel>> CreateSize (SizeModel sizeModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var size = new Size {
                Height = sizeModel.Height,
                Width = sizeModel.Width,
            };

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            var createdSize = new SizeModel {
                Height = size.Height,
                Width = size.Width,
                Id = size.Id,
            };

            return CreatedAtAction(nameof(GetSize), new {id = size.Id}, createdSize);
        } catch (Exception ex){
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SizeModel>>> GetSizes () {
       try {
            var sizes = await _context.Sizes
                .Select( s => new SizeModel {
                    Height = s.Height,
                    Width = s.Width,
                    Id = s.Id,
                })
                .ToListAsync();

            return Ok(sizes);
       } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SizeModel>> GetSize(int id) {
       try {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null) {
                return NotFound();
            }

            var sizeModel = new SizeModel {
                Id = size.Id,
                Height = size.Height,
                Width = size.Width,
            };

            return Ok(sizeModel);
       } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSize(int id) {
        try {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null) {
                return NotFound();
            }

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();

            return NoContent();
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }
}