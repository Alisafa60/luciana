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
    public ColorController(IColorRepository colorRepository) {
        _colorRepository = colorRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ColorModel>> CreateColor(ColorModel colorModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var color = new Color {
                Name = colorModel.Name,
                ParentColorId = colorModel.ParentColorId,
            };

            await _colorRepository.AddAsync(color);

            var createdColorModel = new ColorModel {
                Id = color.Id,
                Name = color.Name,
                HexValue = color.HexValue,
                Description = color.Description,
                ParentColorId = color.ParentColorId
            };

            return CreatedAtAction(nameof(GetColor), new { id = color.Id }, createdColorModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ColorModel>>> GetColors() {
        try {
            var colors = await _colorRepository.GetAllAsync();
            return Ok(colors);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColorModel>> GetColor(int id) {
        try {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null) {
                return NotFound();
            }

            var colorModel = new ColorModel {
                Id = color.Id,
                Name = color.Name,
                Description = color.Description,
                HexValue = color.HexValue,
                ParentColorId = color.ParentColorId
            };

            return Ok(colorModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ColorModel>> GetColorByName(string name) {
        try {
            var color = await _colorRepository.GetByNameAsync(name);
            if (color == null) {
                return NotFound();
            }

            var colorModel = new ColorModel {
                Id = color.Id,
                Name = color.Name,
                Description = color.Description,
                HexValue = color.HexValue,
                ParentColorId = color.ParentColorId,
            };

            return Ok(colorModel);
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
}
