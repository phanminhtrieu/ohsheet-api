using CleanArchitecture.Core.Exceptions.Specifics.MusicTranscriptionExceptions;
using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using CleanArchitecture.Shared;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace CleanArchitecture.Core.Services.MusicTranscriptionServices
{
    public class MusicTranscriptionService(
        AppSettings _appSettings) : IMusicTranscriptionService
    {
        public async Task<ApiResult<string>> TranscribeAsync(IFormFile file)
        {
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10) // Set timeout to 10 minutes
            };

            var allowedExt = new[] { ".mp3", ".ogg", ".wav", ".flac", ".m4a" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExt.Contains(ext))
                throw new MusicTranscriptionInvalidFileException(ext);

            var url = $"{_appSettings.PythonServiceSettings.BaseUrl}/MusicTranscription/transcribe";

            using var content = new MultipartFormDataContent();

            // Thêm file
            using var fileStream = file.OpenReadStream();
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.FileName);

            // Thêm return_base64 field
            content.Add(new StringContent("false"), "return_base64");

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Python API error: {error}");
            }

            var resultString = await response.Content.ReadAsStringAsync();
            return new ApiSuccessResult<string>(resultString);
        }
    }
}
