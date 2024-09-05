using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class PollCreateDto
{
    [Required] public required string Title { get; set; }
    [Required] public required string Content { get; set; }
}
