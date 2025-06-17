namespace IMDBLite.API.Services.Interfaces;

public interface ISupabaseService
{
    Task<string> UploadFileAsync(IFormFile file);
}