using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
    IdentityUserToken<int>>(options)
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserPollOption> UserPollOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //AppUser - AppRole
        builder.Entity<AppUser>()
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId)
            .IsRequired();

        //AppUser - Group
        builder.Entity<UserGroup>().HasKey(x => new {x.UserId, x.GroupId});

        builder.Entity<AppUser>()
            .HasMany(x => x.MemberOf)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder.Entity<Group>()
            .HasMany(x => x.Members)
            .WithOne(x => x.Group)
            .HasForeignKey(x => x.GroupId)
            .IsRequired();

        //Group - Poll
        builder.Entity<Poll>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Polls)
            .HasForeignKey(x => x.GroupId)
            .IsRequired();

        //Poll - PollOption
        builder.Entity<PollOption>()
            .HasOne(x => x.Poll)
            .WithMany(x => x.PollOptions)
            .HasForeignKey(x => x.PollId)
            .IsRequired();

        //AppUser - PollOption
        builder.Entity<UserPollOption>().HasKey(x => new {x.UserId, x.PollOptionId});

        builder.Entity<AppUser>()
            .HasMany(x => x.UserPollOptions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder.Entity<PollOption>()
            .HasMany(x => x.UserPollOption)
            .WithOne(x => x.PollOption)
            .HasForeignKey(x => x.PollOptionId)
            .IsRequired();
    }
}
