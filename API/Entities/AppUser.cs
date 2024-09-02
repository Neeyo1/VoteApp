using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public required string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
    public ICollection<UserGroup> UserGroups { get; set; } = [];
    public Group? GroupOwner { get; set; }
    public ICollection<UserPollOption> UserPollOptions { get; set; } = [];
}