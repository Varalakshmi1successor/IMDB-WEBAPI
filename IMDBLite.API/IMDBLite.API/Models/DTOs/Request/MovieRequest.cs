using System.ComponentModel.DataAnnotations;

namespace IMDBLite.API.Models.DTOs.Request;

public class MovieRequest
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "YearOfRelease is required.")]
    public int YearOfRelease { get; set; }

    [Required(ErrorMessage = "Plot is required.")]
    public string Plot { get; set; }

    [Required(ErrorMessage = "At least one Actor ID is required.")]
    public List<int> ActorIds { get; set; } = new();

    [Required(ErrorMessage = "At least one Genre ID is required.")]
    public List<int> GenreIds { get; set; } = new();

    [Required(ErrorMessage = "Producer ID is required.")]
    public int ProducerId { get; set; }

    [Required(ErrorMessage = "Poster image file is required.")]
    public IFormFile Poster { get; set; }
}