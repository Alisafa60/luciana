using Microsoft.AspNetCore.Mvc;
using backend.Data;
using System.Threading.Tasks;
using backend.Helpers;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging; 

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase{

    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AppDbContext context, IConfiguration configuration, ILogger<AuthController> logger){
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterationModel model){
        
        try{
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            if(_context.Users.Any(u => u.Email == model.Email)){
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
                Gender = model.Gender,
                RoleId = 2
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered Succesfully");

        }catch(Exception){
            return StatusCode(500, "An error occurred while processing the request");
        }
    }

  [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginModel model)
    {
        try{
            
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null){
                return NotFound("Incorrect Credentials");
            }

            if (user?.Role == null){
                    return Unauthorized("Invalid user role");
                }

            if (!PasswordHasher.VerifyPassword(model.Password, user.Password)){
                return Unauthorized("Icorrect Credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JwtSettings:SecretKey"];

            if (string.IsNullOrEmpty(secretKey)){
                 _logger.LogError("Secret key is not configured");
                return StatusCode(500, "Secret key is not configured");
            }

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor{

                Subject = new ClaimsIdentity(new Claim[]{

                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.UserRole.ToString()) 
                }),

                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
        catch (Exception ex){
            _logger.LogError(ex, "An error occurred while processing the request");
            return StatusCode(500, "An error occurred while processing the request");
        }
    }
}