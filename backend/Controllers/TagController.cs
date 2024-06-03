using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/tag")]
[ApiController]
public class TagController : ControllerBase {
    private readonly AppDbContext _context;

    public TagController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TagModel>> CreateTag(TagModel tagModel) {
        if (!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        try {
            var tag = new Tag {
                Name = tagModel.Name,
                Description = tagModel.Description,
                Type = (Tag.TagType)tagModel.Type,
            };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            var createdTag = new TagModel {
                Name = tag.Name,
                Description = tag.Description,
                Type = (TagModel.TagType)tag.Type,
            };

            return CreatedAtAction(nameof(GetTag), new {id = createdTag.Id}, createdTag);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error ${ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagModel>>> GetTags() {
        var tags = await _context.Tags
            .Select(t => new TagModel {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Type = (TagModel.TagType)t.Type,
            }).ToListAsync();

            return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagModel>> GetTag(int id) {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null) {
            return NotFound();
        }

        var tagModel = new TagModel {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            Type = (TagModel.TagType)tag.Type,
        };

        return Ok(tagModel);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTag(int id) {
        try {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error ${ex.Message}");
        }
    }
}