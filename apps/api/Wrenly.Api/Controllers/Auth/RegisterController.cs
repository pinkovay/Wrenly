using Microsoft.AspNetCore.Mvc;
using Wrenly.Application.Auth.Registration;
using Wrenly.Application.Auth.SocialLogin;

namespace Wrenly.Api.Controllers.Auth;

[Route("api/auth")]
[Tags("Auth")]
[ApiController]
public class RegisterController(IUserRegistrationService userRegistrationService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var result = await userRegistrationService.RegisterAsync(registerDTO);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors });
        }
        
        return Ok(new { message = "Usuário registrado com sucesso!" });
    }
}
