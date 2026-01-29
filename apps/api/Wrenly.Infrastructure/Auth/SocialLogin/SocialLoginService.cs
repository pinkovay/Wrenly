using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Wrenly.Application.Auth.SocialLogin;
using Wrenly.Application.Common.Security;
using Wrenly.Domain.Common.Results;
using Wrenly.Domain.Entities;
using Wrenly.Domain.ValueObjects;

namespace Wrenly.Infrastructure.Auth.SocialLogin;

public class SocialLoginService(
    UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService) : ISocialLoginService
{
    public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
    {
        return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    }

    public async Task<Result<SocialUserDTO>> GetExternalLoginInfoAsync()
    {
        var info = await signInManager.GetExternalLoginInfoAsync();
        
        if (info == null) 
            return Result<SocialUserDTO>.Failure("Falha ao obter informações do provedor externo.");

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        
        if (string.IsNullOrEmpty(email))
            return Result<SocialUserDTO>.Failure("Email não fornecido pelo provedor.");

        return Result<SocialUserDTO>.Success(new SocialUserDTO(
            Email: email,
            Provider: info.LoginProvider,
            ProviderKey: info.ProviderKey
        ));
    }

    public async Task<Result<SocialAuthResponseDTO>> ProcessSocialLoginAsync(SocialUserDTO socialUserDTO)
    {
        var user = await userManager.FindByLoginAsync(socialUserDTO.Provider, socialUserDTO.ProviderKey);
        if (user != null)
        {
            return Result<SocialAuthResponseDTO>.Success(new SocialAuthResponseDTO(
            RequiresProfileCompletion: false, 
            Email: user.Email!, 
            Token: tokenService.GenerateJwt(user)));
        }

        // Vinculo não encontrado, mas email já existe na base?
        var userByEmail = await userManager.FindByEmailAsync(socialUserDTO.Email);
        
        if (userByEmail != null)
        {
            await userManager.AddLoginAsync(userByEmail, 
                new UserLoginInfo(socialUserDTO.Provider, socialUserDTO.ProviderKey, socialUserDTO.Provider));
            
            return Result<SocialAuthResponseDTO>.Success(new SocialAuthResponseDTO(
            RequiresProfileCompletion: false, 
            Email: userByEmail.Email!, 
            Token: tokenService.GenerateJwt(userByEmail)));
        }

        // Usuário é novo mesmo. Avisa o Front que precisa escolher DisplayName/Username
            return Result<SocialAuthResponseDTO>.Success(new SocialAuthResponseDTO(
            RequiresProfileCompletion: true,
            Email: socialUserDTO.Email,
            Provider: socialUserDTO.Provider,
            ProviderKey: socialUserDTO.ProviderKey
        ));
    }

    public async Task<Result<SocialAuthResponseDTO>> FinalizeRegistrationAsync(FinalizeSocialRegisterDTO finalizeDTO)
    {
        var displayNameResult = DisplayName.Create(finalizeDTO.DisplayName);
        if (!displayNameResult.Succeeded)
            return Result<SocialAuthResponseDTO>.Failure(displayNameResult.Errors);

        var user = new User
        {
            Email = finalizeDTO.Email,
            UserName = finalizeDTO.Email,
            DisplayName = displayNameResult.Data!
        };

        var createUser = await userManager.CreateAsync(user);

        if (!createUser.Succeeded) 
            return Result<SocialAuthResponseDTO>.Failure(createUser.Errors.Select(e => e.Description).ToArray());

        var loginResult = await userManager.AddLoginAsync(user, 
            new UserLoginInfo(finalizeDTO.Provider, finalizeDTO.ProviderKey, finalizeDTO.Provider));

        if (!loginResult.Succeeded)
            return Result<SocialAuthResponseDTO>.Failure("Erro ao vincular conta externa.");
        
        var token = tokenService.GenerateJwt(user);

        return Result<SocialAuthResponseDTO>.Success(new SocialAuthResponseDTO(
            RequiresProfileCompletion: false,
            Email: user.Email!,
            Token: token
        ));
    }
}   