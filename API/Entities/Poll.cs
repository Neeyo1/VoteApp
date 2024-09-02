using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Polls")]
public class Poll
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public IEnumerable<PollOption> PollOptions { get; set; } = [];
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
