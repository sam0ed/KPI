using Classroom.WebAPI.Models;
using Classroom.WebAPI.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Classroom.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private const string Token =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

    [SwaggerResponse(200, "Register success", typeof(AuthSuccessDTO))]
    [SwaggerResponse(400, "User with this email already exists", typeof(ErrorModel))]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDTO dto)
    {
        await Task.Delay(1);
        return Ok(new AuthSuccessDTO
        {
            Token = Token
        });
    }

    [SwaggerResponse(200, "Login success", typeof(AuthSuccessDTO))]
    [SwaggerResponse(400, "User with this email does not exist", typeof(ErrorModel))]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        await Task.Delay(1);
        return Ok(new AuthSuccessDTO
        {
            Token = Token
        });
    }
}