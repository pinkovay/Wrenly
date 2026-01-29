using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wrenly.Application.Auth.SocialLogin;
namespace Wrenly.Api.Controllers.Auth;

[ApiController]
[Tags("OAuth")]
[Route("api/auth")]
public class SocialAuthController(ISocialLoginService socialLoginService) : ControllerBase
{
    // 1. Inicia o desafio (Challenge) para o provedor (Google, Facebook, etc)
    [HttpGet("signin-{provider}")]
    public IActionResult ExternalLogin(string provider)
    {
        var redirectUrl = Url.RouteUrl("SocialCallback", null, "https");
        
        var properties = socialLoginService.ConfigureExternalAuthenticationProperties(provider, redirectUrl!);
        
        return Challenge(properties, provider);
    }

    [HttpGet("callback", Name = "SocialCallback")]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var infoResult = await socialLoginService.GetExternalLoginInfoAsync();
        if (!infoResult.Succeeded) return BadRequest(infoResult.Errors);

        var result = await socialLoginService.ProcessSocialLoginAsync(infoResult.Data!);
        if (!result.Succeeded) return BadRequest(result.Errors);

        const string frontendBaseUrl = "http://localhost:5173";

        if (result.Data!.RequiresProfileCompletion)
        {
            return Redirect($"{frontendBaseUrl}/complete-profile?" +
                            $"email={result.Data.Email}&" +
                            $"provider={infoResult.Data!.Provider}&" +
                            $"key={infoResult.Data.ProviderKey}");
        }

        return Redirect($"{frontendBaseUrl}/login-success?token={result.Data.Token}");
    }

    [HttpPost("finalize")]
    public async Task<IActionResult> FinalizeRegistration([FromBody] FinalizeSocialRegisterDTO finalizeDTO)
    {
        var result = await socialLoginService.FinalizeRegistrationAsync(finalizeDTO);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }
}

