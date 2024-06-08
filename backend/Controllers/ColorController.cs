using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/color")]
[ApiController]
public class ColorController : ControllerBase {
    private readonly IColorRepository _colorRepository;
    private readonly IAttributeService _attributeService;
    public ColorController(IColorRepository colorRepository, IAttributeService attributeService) {
        _colorRepository = colorRepository;
        _attributeService = attributeService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ColorDto>> CreateColor(ColorDto colorDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var color = new Color {
                Name = colorDto.Name,
                ParentColorId = colorDto.ParentColorId,
            };

            await _colorRepository.AddAsync(color);

            var createdColorDto = new ColorDto {
                Id = color.Id,
                Name = color.Name,
                HexValue = color.HexValue,
                Description = color.Description,
                ParentColorId = color.ParentColorId
            };

            return CreatedAtAction(nameof(GetColor), new { id = color.Id }, createdColorDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ColorDto>>> GetColors() {
        try {
            var colors = await _colorRepository.GetAllAsync();
            return Ok(colors);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColorDto>> GetColor(int id) {
        try {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null) {
                return NotFound();
            }

            var colorDto = new ColorDto {
                Id = color.Id,
                Name = color.Name,
                Description = color.Description,
                HexValue = color.HexValue,
                ParentColorId = color.ParentColorId
            };

            return Ok(colorDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ColorDto>> GetColorByName(string name) {
        try {
            var color = await _colorRepository.GetByNameAsync(name);
            if (color == null) {
                return NotFound();
            }

            var colorDto = new ColorDto {
                Id = color.Id,
                Name = color.Name,
                Description = color.Description,
                HexValue = color.HexValue,
                ParentColorId = color.ParentColorId,
            };

            return Ok(colorDto);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteColor(int id) {
        try {
            await _colorRepository.DeleteAsync(id);
            return NoContent();

        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPost("many")]
    public async Task<IActionResult> GetColorByIds([FromBody] IdsDto idsDto ) {
        try {
            var colorNames = await _attributeService.GetColorNames(idsDto.Ids);
            return Ok(colorNames);
        } catch (Exception ex){
            return StatusCode(500, $"Internal Server Error ${ex.Message}");
        }
    }
}
