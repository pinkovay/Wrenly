namespace Wrenly.Application.Auth.SocialLogin;

public record SocialUserDTO(
    string Email, 
    string Provider, 
    string ProviderKey);


// DTO para a resposta que vai para o Frontend decidir o fluxo
public record SocialAuthResponseDTO(
    bool RequiresProfileCompletion,
    string Email,
    string? Provider = null,
    string? ProviderKey = null,
    string? Token = null);


// DTO para o passo final, onde o usuário escolhe o DisplayName
public record FinalizeSocialRegisterDTO(
    string Email,
    string Provider,
    string ProviderKey,
    string DisplayName);