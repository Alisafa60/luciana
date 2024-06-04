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
    public async Task<ActionResult<ParentColorModel>> CreateParentColor(ParentColorModel parentColorModel) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        try {
            var parentColor = new ParentColor {
                Name = parentColorModel.Name,
            };

            await _parentColorRepository.AddAsync(parentColor);

            var createdColorModel = new ParentColorModel {
                Name = parentColor.Name,
                Id = parentColor.Id,
            };

            return CreatedAtAction(nameof(GetParentColor), new { id = parentColor.Id }, parentColor);
        } catch (Exception ex){
            return StatusCode(500, $"Internal server error {ex.Message}");
       }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParentColorModel>>> GetParentColors() {
       try{
            var parentColors = await _parentColorRepository.GetAllAsync();
            return Ok(parentColors);

       } catch(Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
       }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParentColorModel>> GetParentColor(int id) {
        try {
            var parentColor = await _parentColorRepository.GetByIdAsync(id);

            var parentColorModel = new ParentColorModel {
                Id = parentColor.Id,
                Name = parentColor.Name,
            };

            return Ok(parentColorModel);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ParentColorModel>> GetParentColorByName(string name) {
        try {
            var parentColor = await _parentColorRepository.GetByNameAsync(name);
            if (parentColor == null) {
                return NotFound();
            }

            var parentColorModel = new ParentColorModel {
                Id = parentColor.Id,
                Name = parentColor.Name
            };

            return Ok(parentColorModel);
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

}