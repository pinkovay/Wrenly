using System;
using Wrenly.Domain.Entities;

namespace Wrenly.Application.Common.Security;

public interface ITokenService
{
    string GenerateJwt(User user);
}
