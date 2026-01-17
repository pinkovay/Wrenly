using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wrenly.Domain.Entities;

namespace Wrenly.Infrastructure.Auth.Identity;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasIndex(u => u.Email).IsUnique();
            b.HasIndex(u => u.UserName).IsUnique();
            
            b.Property(u => u.Email).HasMaxLength(256).IsRequired();
            b.Property(u => u.UserName).HasMaxLength(256).IsRequired();
        });
        
        builder.Entity<IdentityRole<Guid>>(b => b.ToTable("Roles"));
        
        builder.Entity<IdentityUserRole<Guid>>(b => b.ToTable("UserRoles"));
        
        builder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("UserClaims"));
        
        builder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("UserLogins"));
        
        builder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("RoleClaims"));
        
        builder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("UserTokens"));
    }
}
