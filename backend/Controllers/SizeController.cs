using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/size")]
[ApiController]
public class SizeController : ControllerBase {
    private readonly ISizeRepository _sizeRepository;
    public SizeController(ISizeRepository sizeRepository) {
        _sizeRepository = sizeRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SizeDto>> CreateSize (SizeDto sizeDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var size = new Size {
                Height = sizeDto.Height,
                Width = sizeDto.Width,
            };

            await _sizeRepository.AddAsync(size);

            var createdSize = new SizeDto {
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
    public async Task<ActionResult<IEnumerable<SizeDto>>> GetSizes () {
       try {
            var sizes = await _sizeRepository.GetAllAsync();

            return Ok(sizes);
       } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SizeDto>> GetSize(int id) {
       try {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size == null) {
                return NotFound();
            }

            var sizeDto = new SizeDto {
                Id = size.Id,
                Height = size.Height,
                Width = size.Width,
            };

            return Ok(sizeDto);
       } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSize(int id) {
        try {
            await _sizeRepository.DeleteAsync(id);
            return NoContent();
            
        } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }
}