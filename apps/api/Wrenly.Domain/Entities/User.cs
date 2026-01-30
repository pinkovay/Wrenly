using System;
using Microsoft.AspNetCore.Identity;
using Wrenly.Domain.ValueObjects;

namespace Wrenly.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public string DisplayName {get; set;} = null!;
}
