using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("UserPollOptions")]
public class UserPollOption
{
    public int UserId { get; set; }
    public int PollOptionId { get; set; }
    public AppUser User { get; set; } = null!;
    public PollOption PollOption { get; set; } = null!;
}
