using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Core.Interfaces.UserServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using PuppeteerSharp;

namespace CleanArchitecture.Core.Services.MusicSheetServices
{
    public class MusicSheetService(
        IMusicSheetRepository _musicSheetRepository,
        IUnitOfWork _unitOfWork,
        ICurrentUserService _currentUserService,
        IBlobService _blobService) : IMusicSheetService
    {
        public async Task<ApiResult<MusicSheetResponse>> GetMusicSheetByIdAsync(int id)
        {
            var musicSheetResponse = await _musicSheetRepository.GetDetailByIdAsync(id, _currentUserService.UserGuid);

            if (musicSheetResponse == null) throw new UserFriendlyException(
                ErrorCode.NotFound,
                "Music Sheet is not found", 
                "Music Sheet is not found");

            musicSheetResponse.ViewCount++; // Just increment view count for response, this value is not stored in db

            return new ApiSuccessResult<MusicSheetResponse>(musicSheetResponse);
        }

        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> ListByPagingAsync(MusicSheetPagingRequest request, CancellationToken cancellationToken)
        {
            var pagedResult = await _musicSheetRepository.ListByPagingAsync(request, _currentUserService.UserGuid, cancellationToken);

            return new ApiSuccessResult<DataTablePagedResult<MusicSheetResponse>>(pagedResult);
        }

        public async Task<ApiResult<int>> CreateMusicSheetAsync(MusicSheetRequest request, CancellationToken cancellationToken)
        {
            string? thumbnailUrl = null;
            if (request.ThumbnailFile != null)
            {
                using var stream = request.ThumbnailFile.OpenReadStream();
                thumbnailUrl = await _blobService.UploadAsync(stream, request.ThumbnailFile.FileName, request.ThumbnailFile.ContentType);
            }

            var musicSheet = MusicSheet.Create(
               request.UserId,
               request.Title,
               request.Description,
               request.TranscriptionId,
               thumbnailUrl
           );

            if (request.Tags != null && request.Tags.Any())
            {
                var normalizedTags = request.Tags
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (normalizedTags.Any())
                {
                    var existingTags = await _musicSheetRepository.GetTagsByNamesAsync(normalizedTags, cancellationToken);
                    var existingTagNames = existingTags.Select(t => t.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

                    var newTags = normalizedTags
                        .Where(t => !existingTagNames.Contains(t))
                        .Select(t => new MusicSheetTag(t))
                        .ToList();

                    musicSheet.AddTags(existingTags.Concat(newTags));
                }
            }

            await _musicSheetRepository.AddAsync(musicSheet);

            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync());
        }

        public async Task<ApiResult<int>> UpdateMusicSheetAsync(int id, MusicSheetRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserGuid;
            if (userId == null) return new ApiErrorResult<int>("User not authenticated");

            var musicSheet = await _musicSheetRepository.GetWithTagsAsync(id, cancellationToken);

            if (musicSheet == null) return new ApiErrorResult<int>("Music Sheet not found");

            if (musicSheet.UserId != userId) return new ApiErrorResult<int>("You are not authorized to edit this music sheet");
            musicSheet.UpdateMetadata(request.Title, request.Description);

            if (request.Tags != null)
            {
                var normalizedTags = request.Tags
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var existingTags = await _musicSheetRepository.GetTagsByNamesAsync(normalizedTags, cancellationToken);
                var existingTagNames = existingTags.Select(t => t.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

                var newTags = normalizedTags
                    .Where(t => !existingTagNames.Contains(t))
                    .Select(t => new MusicSheetTag(t))
                    .ToList();

                musicSheet.UpdateTags(existingTags.Concat(newTags));
            }
            else 
            {
                 if (request.Tags != null)
                 {
                     musicSheet.UpdateTags(new List<MusicSheetTag>());
                 }
            }
            
            _musicSheetRepository.Update(musicSheet);
            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync());
        }

        public async Task<ApiResult<int>> LikeAsync(int sheetId, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserGuid;
            if (userId == null) return new ApiErrorResult<int>("User not authenticated");

            var sheet = await _musicSheetRepository.GetWithLikesAsync(sheetId, cancellationToken);
            if (sheet == null) return new ApiErrorResult<int>("Music sheet not found");

            sheet.AddLike(userId.Value);

            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync());
        }

        public async Task<ApiResult<int>> UnlikeAsync(int sheetId, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserGuid;
            if (userId == null) return new ApiErrorResult<int>("User not authenticated");

            var sheet = await _musicSheetRepository.GetWithLikesAsync(sheetId, cancellationToken);
            if (sheet == null) return new ApiErrorResult<int>("Music sheet not found");

            sheet.RemoveLike(userId.Value);

            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync());
        }

        public async Task<FileResponse> ExportToImageAsync(string htmlContent)
        {
            try
            {
                var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync(); 

                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
                });

                await using var page = await browser.NewPageAsync();

                // Set content
                await page.SetContentAsync(htmlContent);

                // ✅ Tùy chọn: tự động lấy kích thước trang để screenshot full page
                var width = await page.EvaluateExpressionAsync<int>("document.body.scrollWidth"); // ✅
                var height = await page.EvaluateExpressionAsync<int>("document.body.scrollHeight"); // ✅

                await page.SetViewportAsync(new()
                {
                    Width = width,  
                    Height = height 
                });

                // Screenshot
                var screenshotOptions = new ScreenshotOptions
                {
                    Type = ScreenshotType.Png,
                    FullPage = true
                };

                var bytes = await page.ScreenshotDataAsync(screenshotOptions);
                return new FileResponse { File = bytes };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ErrorCode.Internal, ex.Message, "Failed to export music sheet to image", ex);
            }
        }
    }
}
