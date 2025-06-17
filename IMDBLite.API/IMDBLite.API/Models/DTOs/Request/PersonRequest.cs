using System.ComponentModel.DataAnnotations;
using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Models.DTOs.Request;

public class PersonRequest
{
    [Required(ErrorMessage = "Name is required,It cannot be empty")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Bio is required.")]
    public string Bio { get; set; }

    [Required(ErrorMessage = "Date of Birth must be provided,It should not be Empty")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    public Gender? Gender { get; set; }
}