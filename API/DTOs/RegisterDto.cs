﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [MaxLength(50)]
    public string? Username { get; set; }
    [Required]
    public string? KnownAs { get; set; }
    [Required]
    public string? Gender { get; set; }
    [Required]
    public string? DateOfBirth { get; set; }
    [Required]
    public string? City { get; set; }
    [Required]
    public string? Country { get; set; }
    [Required]
    public string?  Password { get; set; }
}
