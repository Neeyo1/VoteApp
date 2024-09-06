using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class PollOptionCreateDto
{
    [Required] public required string Option { get; set; }
}
