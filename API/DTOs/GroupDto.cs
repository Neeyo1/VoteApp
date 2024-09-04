using System;
using API.Entities;

namespace API.DTOs;

public class GroupDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<MemberDto> Members { get; set; } = [];
    public required string Owner { get; set; }
    public List<PollDto> Polls { get; set; } = [];
}
