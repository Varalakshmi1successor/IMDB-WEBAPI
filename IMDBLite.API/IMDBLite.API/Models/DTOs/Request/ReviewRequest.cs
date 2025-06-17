using System.ComponentModel.DataAnnotations;

namespace IMDBLite.API.Models.DTOs.Request;

public class ReviewRequest
{
    [Required(ErrorMessage = "Message is required.")]
    public string Message { get; set; }
}