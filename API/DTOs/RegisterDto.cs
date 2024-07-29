using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; } = string.Empty;
    [Required]
    public required string  Password { get; set; } = string.Empty;
}
