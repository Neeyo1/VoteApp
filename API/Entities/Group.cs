using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Groups")]
public class Group
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Owner { get; set; }
    public ICollection<UserGroup> Members { get; set; } = [];
    public ICollection<Poll> Polls { get; set; } = [];
}
