using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

public class AuthMiddleware{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger; 
    private readonly JwtSettings _jwtSettings;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger, IOptions<JwtSettings> jwtSettings){
        _next = next;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task Invoke(HttpContext context) {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null) {
            AttachUserToContext(context, token);
        }

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claims = jwtToken.Claims;
            var identity = new ClaimsIdentity(claims, "jwt");
            context.User = new ClaimsPrincipal(identity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to authinticate jwt token");
        }
    }
}