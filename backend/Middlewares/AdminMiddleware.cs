using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class AdminMiddleware{
    private readonly RequestDelegate _next;
    private readonly ILogger<AdminMiddleware> _logger;

    public AdminMiddleware(RequestDelegate next, ILogger<AdminMiddleware> logger){
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context){
        if (!context.User.Identity?.IsAuthenticated ?? false){
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (context.User.IsInRole("Admin")){
            await _next.Invoke(context);
        }else{
            _logger.LogInformation("Unauthorized access to admin endpoint by user: {User}", context.User.Identity.Name);
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("You are not authorized to access this resource.");
        }
    }
}