using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/TexturePattern")]
[ApiController]
public class TexturePatternController : ControllerBase {
    private readonly ITexturePatternRepository _texturePatternRepository;
    public TexturePatternController(ITexturePatternRepository texturePatternRepository) {
        _texturePatternRepository = texturePatternRepository;
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

            await _texturePatternRepository.AddAsync(texturePattern);

            var createdTP = new TexturePatternModel {
                Name = texturePattern.Name,
                Id = texturePattern.Id,
            };

            return CreatedAtAction(nameof(GetTexturePattern), new {id = texturePatternModel.Id}, createdTP);
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TexturePatternModel>>> GetTexturePatterns () {
        try {
            var texturePatterns = await _texturePatternRepository.GetAllAsync();

            return Ok(texturePatterns);
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TexturePatternModel>> GetTexturePattern (int id) {
        try {
            var texturePattern = await _texturePatternRepository.GetByIdAsync(id);
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
        try{
            await _texturePatternRepository.DeleteAsync(id);

            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
       
    }
}
