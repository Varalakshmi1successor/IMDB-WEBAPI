using System.ComponentModel.DataAnnotations;

namespace IMDBLite.API.Models.DTOs.Request;

public class GenreRequest
{
    [Required(ErrorMessage = "Genre name is required.")]
    public string Name { get; set; }
}