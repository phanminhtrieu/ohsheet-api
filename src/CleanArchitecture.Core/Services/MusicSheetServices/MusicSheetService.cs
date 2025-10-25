using Azure.Core;
using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.FileStorageService;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using System.Net.WebSockets;

namespace CleanArchitecture.Core.Services.MusicSheetServices
{
    public class MusicSheetService(
        IMusicSheetRepository _musicSheetRepository,
        IUnitOfWork _unitOfWork,
        IFileStorageService _fileStorageService) : IMusicSheetService
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
                FilePath = musicSheet.FilePath,
                FileSize = musicSheet.FileSize,
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
               request.ParentId,
               request.Title,
               request.Description,
               request.IsForked,
               request.Status,
               request.MusicSheetVisibility
           );

            if (request.Tags != null)
            {
                bool isExistingTag = request.Tags != null && request.Tags.Any();

                if (isExistingTag)
                {
                    musicSheet.AddTags(request.Tags!);
                }
            }

            if (request.MidiFile != null)
            {
                byte[] midiBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await request.MidiFile.CopyToAsync(memoryStream);
                    midiBytes = memoryStream.ToArray();
                }

                musicSheet.AttachBinary(midiBytes);

                string filePath = await  _fileStorageService.SaveMidiFileAsync(request.UserId, midiBytes);
                musicSheet.SetFilePath(filePath);

                long fileSize = await _fileStorageService.GetFileSizeAsync(filePath);
                musicSheet.SetFileSize(fileSize);
            }

            await _musicSheetRepository.AddAsync(musicSheet);

            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync());
        }
    }
}
