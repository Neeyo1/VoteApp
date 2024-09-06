using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Polls")]
public class Poll
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool HasStarted { get; set; } = false;
    public bool HasEnded { get; set; } = false;
    public ICollection<PollOption> PollOptions { get; set; } = [];
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
