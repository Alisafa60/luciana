using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/tag")]
[ApiController]
public class TagController : ControllerBase {
    private readonly ITagRepository _tagRepository;

    public TagController(ITagRepository tagRepository) {
        _tagRepository = tagRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TagDto>> CreateTag(TagDto tagDto) {
        if (!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        try {
            var tag = new Tag {
                Name = tagDto.Name,
                Description = tagDto.Description,
                Type = (Tag.TagType)tagDto.Type,
            };

            await _tagRepository.AddAsync(tag);

            var createdTag = new TagDto {
                Name = tag.Name,
                Description = tag.Description,
                Type = (TagDto.TagType)tag.Type,
            };

            return CreatedAtAction(nameof(GetTag), new {id = createdTag.Id}, createdTag);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error ${ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags() {
        var tags = await _tagRepository.GetAllAsync();
            return Ok(tags);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(int id) {
        var tag = await _tagRepository.GetByIdAsync(id);

        var tagDto = new TagDto {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            Type = (TagDto.TagType)tag.Type,
        };

        return Ok(tagDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTag(int id) {
        try {
            await _tagRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error ${ex.Message}");
        }
    }
}