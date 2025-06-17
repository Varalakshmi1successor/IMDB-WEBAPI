using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Settings;
using Microsoft.Extensions.Options;
using Supabase;
using FileOptions = Supabase.Storage.FileOptions;

namespace IMDBLite.API.Services;

public class SupabaseService : ISupabaseService
{
    private readonly Client _client;
    private readonly SupabaseSettings _settings;

    public SupabaseService(IOptions<SupabaseSettings> settings)
    {
        _settings = settings.Value;
        _client = new Client(_settings.SupabaseUrl, _settings.SupabaseKey);
        _client.InitializeAsync().Wait();
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var path = $"public/{fileName}";
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }

        var storageBucket = _client.Storage.From(_settings.StorageBucket);
        var uploadResponse = await storageBucket.Upload(fileBytes, path, new FileOptions
        {
            Upsert = true,
            ContentType = file.ContentType
        });
        if (uploadResponse.Contains("error")) throw new Exception($"File upload failed. Error: {uploadResponse}");
        return $"{_settings.SupabaseUrl}/storage/v1/object/{_settings.StorageBucket}/{path}";
    }
}