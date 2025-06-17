using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Models.DTOs.Response;

public class PersonResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
}