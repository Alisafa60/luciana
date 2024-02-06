using Microsoft.AspNetCore.Mvc;
using backend.Data;
using System.Threading.Tasks;
using backend.Helpers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase{

    private readonly AppDbContext _context;

    public AuthController(AppDbContext context){
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterationModel model){
        
        try{
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            if(_context.Users.Any(user => user.Email == model.Email)){
                return Conflict("Email is already registered");
            }

            string hashedPassword = PasswordHasher.HashPassword(model.Password);

            var user = new User{
                Email = model.Email,
                Password = hashedPassword,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth.ToUniversalTime(),
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered Succesfully");

        }catch(Exception){
            return StatusCode(500, "An error occurred while processing the request");
        }
    }
       
}