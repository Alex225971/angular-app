﻿namespace API;

public class UserDto
{
    public required string Username { get; set; }
    public string Token { get; set; }
    public string? PhotoUrl { get; set; }
}
