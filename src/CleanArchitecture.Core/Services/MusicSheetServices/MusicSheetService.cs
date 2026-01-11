using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.FileStorageService;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using PuppeteerSharp;

namespace CleanArchitecture.Core.Services.MusicSheetServices
{
    public class MusicSheetService(
        IMusicSheetRepository _musicSheetRepository,
        IUnitOfWork _unitOfWork) : IMusicSheetService
    {
        public async Task<ApiResult<MusicSheetResponse>> GetMusicSheetByIdAsync(int id)
        {
            var musicSheet = await _musicSheetRepository.FindByIdAsync(id);

            if (musicSheet == null) throw new UserFriendlyException(
                ErrorCode.NotFound,
                "Music Sheet is not found", 
                "Music Sheet is not found");

            var musicSheetResponse = new MusicSheetResponse() 
            { 
                Id = musicSheet.Id,
                UserId = musicSheet.UserId,
                Title = musicSheet.Title,
                ParentId = musicSheet.ParentId,
                Description = musicSheet.Description,
                TranscriptionId = musicSheet.TranscriptionId,
                Status = musicSheet.Status,
                MusicSheetVisibility = musicSheet.MusicSheetVisibility,
                MidiData = musicSheet.MidiData,
                ViewCount = musicSheet.ViewCount + 1, // Just increment view count for response, this value is not stored in db
                LikeCount = musicSheet.LikeCount,
                CommentCount = musicSheet.CommentCount,
                ShareCount = musicSheet.ShareCount,
                IsForked = musicSheet.IsForked,
                Comments = musicSheet.Comments,
                Likes = musicSheet.Likes,
                Tags = musicSheet.Tags,
                CreatedDate = musicSheet.CreatedDate,
                ModifiedDate = musicSheet.ModifiedDate,
            }; 

            return new ApiSuccessResult<MusicSheetResponse>(musicSheetResponse);
        }

        public async Task<ApiResult<int>> CreateMusicSheetAsync(MusicSheetRequest request, CancellationToken cancellationToken)
        {
            var musicSheet = MusicSheet.Create(
               request.UserId,
               request.Title,
               request.Description,
               request.TranscriptionId
           );

            await _musicSheetRepository.AddAsync(musicSheet);

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
