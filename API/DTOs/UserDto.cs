namespace API;

public class UserDto
{
    public required string Username { get; set; }
    public string Token { get; set; }
    public required string KnownAs { get; set; }
    public required string Gender { get; set; }
    public string? PhotoUrl { get; set; }
}
