namespace API.DTOs;

public class PollOptionDto
{
    public int Id { get; set; }
    public required string Option { get; set; }
    public int VotesCount { get; set; }
    public List<MemberDto> UserPollOption { get; set; } = [];
}
