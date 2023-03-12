using Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Context;

public class IdentityContext : IdentityDbContext<ApplicationUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        _ = builder.Entity<ApplicationUser>()
            .ToTable("Users");

        _ = builder.Entity<IdentityRole>()
            .ToTable("Roles");

        _ = builder.Entity<IdentityUserClaim<string>>()
            .ToTable("UserClaims");

        _ = builder.Entity<IdentityUserRole<string>>()
            .ToTable("UserRoles");

        _ = builder.Entity<IdentityUserLogin<string>>()
            .ToTable("UserLogins");

        _ = builder.Entity<IdentityRoleClaim<string>>()
            .ToTable("RoleClaims");

        _ = builder.Entity<IdentityUserToken<string>>()
            .ToTable("UserTokens");
    }
}