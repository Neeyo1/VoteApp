using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Groups")]
public class Group
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; } = [];
    public int OwnerId { get; set; }
    public AppUser Owner { get; set; } = null!;
    public IEnumerable<Poll> Polls { get; set; } = [];
}
