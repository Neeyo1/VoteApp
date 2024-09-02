using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("PollOptions")]
public class PollOption
{
    public int Id { get; set; }
    public required string Option { get; set; }
    public int VotesCount { get; set; }
    public int PollId { get; set; }
    public Poll Poll { get; set; } = null!;
    public ICollection<UserPollOption> UserPollOption { get; set; } = [];
}
