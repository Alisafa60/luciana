using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/parentcolor")]
[ApiController]
public class ParentColorController : ControllerBase {
    private readonly IParentColorRepository _parentColorRepository;

    public ParentColorController(IParentColorRepository parentColorRepository) {
        _parentColorRepository = parentColorRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ParentColorDto>> CreateParentColor(ParentColorDto parentColorDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var parentColor = MapToParentColor(parentColorDto);
            await _parentColorRepository.AddAsync(parentColor);
            var createdColorModel = MapToParentColorDto(parentColor);

            return CreatedAtAction(nameof(GetParentColor), new { id = parentColor.Id }, parentColor);
        } catch (Exception ex){
            return StatusCode(500, $"Internal server error {ex.Message}");
       }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentColorDto>>> GetParentColors() {
       try{
            var parentColors = await _parentColorRepository.GetAllAsync();
            return Ok(parentColors);

       } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentColorDto>> GetParentColor(int id) {
        try {
            var parentColor = await _parentColorRepository.GetByIdAsync(id);

            var parentColorDto = MapToParentColorDto(parentColor);

            return Ok(parentColorDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentColorDto>> GetParentColorByName(string name) {
        try {
            var parentColor = await _parentColorRepository.GetByNameAsync(name);
            if (parentColor == null) {
                return NotFound();
            }

            var parentColorDto = MapToParentColorDto(parentColor);

            return Ok(parentColorDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteById(int id) {
        try{
            await _parentColorRepository.DeleteAsync(id);
            return NoContent();
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    private ParentColor MapToParentColor(ParentColorDto parentColorDto) {
        return new ParentColor {
            Id = parentColorDto.Id,
            Name = parentColorDto.Name,
        };
    }

    private ParentColorDto MapToParentColorDto(ParentColor parentColor) {
        return new ParentColorDto {
            Id = parentColor.Id,
            Name = parentColor.Name,
        };
    }

}