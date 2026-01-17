using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wrenly.Application.Auth.DTOs;
using Wrenly.Application.Auth.Interfaces;

namespace Wrenly.Api.Controllers.Auth;

[Route("api/auth")]
[ApiController]
public class RegisterController(IUserRegistrationService userRegistrationService) : ControllerBase
{
    [HttpPost("singup")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await userRegistrationService.RegisterAsync(request);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(new { message = "Usuário registrado com sucesso!" });
    }
}
