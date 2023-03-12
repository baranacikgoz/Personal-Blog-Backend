using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; } = null!;
    public required string LastName { get; set; } = null!;
}