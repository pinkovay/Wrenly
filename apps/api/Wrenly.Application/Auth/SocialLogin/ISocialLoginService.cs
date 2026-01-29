using System;
using Microsoft.AspNetCore.Authentication;
using Wrenly.Domain.Common.Results;

namespace Wrenly.Application.Auth.SocialLogin;

public interface ISocialLoginService
{
    // Gera as propriedades para o "Challenge"
    AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);

    // Extrai os dados que o provedor mandou após o callback
    Task<Result<SocialUserDTO>> GetExternalLoginInfoAsync();

    // Verifica se o usuário já existe ou se precisa de finalização
    Task<Result<SocialAuthResponseDTO>> ProcessSocialLoginAsync(SocialUserDTO socialUserDTO);
    
    // Efetiva a criação do usuário com o DisplayName escolhido
    Task<Result<SocialAuthResponseDTO>> FinalizeRegistrationAsync(FinalizeSocialRegisterDTO finalizeDTO);
}
