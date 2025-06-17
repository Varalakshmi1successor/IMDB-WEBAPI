namespace IMDBLite.API.Models.DTOs.Response;

public class LoginResponse
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string FullName { get; set; }
    public string Message { get; set; } = "Login successful";
}