using System;

namespace API.DTOs;

public class PollDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public List<PollOptionDto> PollOptions { get; set; } = [];
}
