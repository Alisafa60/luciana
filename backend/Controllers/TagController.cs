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

            await _tagRepository.AddAsync(tag);

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
        var tags = await _tagRepository.GetAllAsync();
            return Ok(tags);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagModel>> GetTag(int id) {
        var tag = await _tagRepository.GetByIdAsync(id);

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
            await _tagRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error ${ex.Message}");
        }
    }
}