using CleanArchitecture.Core.Domain.Models.MusicTranscription;
using CleanArchitecture.Core.Exceptions.Specifics.MusicTranscriptionExceptions;
using CleanArchitecture.Core.Interfaces.MusicTranscriptionServices;
using CleanArchitecture.Shared;
using CleanArchitecture.Shared.Common.Errors.Messages;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace CleanArchitecture.Core.Services.MusicTranscriptionServices
{
    public class MusicTranscriptionService(
        AppSettings _appSettings) : IMusicTranscriptionService
    {
        public async Task<ApiResult<MusicTranscriptionResponse>> TranscribeAsync(IFormFile file)
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

            // Thêm file path
            content.Add(new StringContent(_appSettings.PythonServiceSettings.OutputFolder), "file_path");


            var response = new HttpResponseMessage();
            try
            {
                response = await httpClient.PostAsync(url, content);

                var resultString = await response.Content.ReadAsStringAsync();
                var resultJson = JsonSerializer.Deserialize<MusicTranscriptionResponse>(resultString);

                if (resultJson == null) return new ApiErrorResult<MusicTranscriptionResponse>(ErrorMessage.InternalServerError);

                return new ApiSuccessResult<MusicTranscriptionResponse>(resultJson);
            }
            catch
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Python API error: {error}");
            }
        }
    }
}
