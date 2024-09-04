using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class GroupCreateDto
{
    [Required] public required string Name { get; set; }
}
