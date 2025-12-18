using System;

namespace Wrenly.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id {get; protected set; } = Guid.NewGuid();
}
