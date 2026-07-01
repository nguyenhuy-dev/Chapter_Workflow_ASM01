using MangaWorkflow.Entities.HuyNQ.Models;
using MangaWorkflow.Repositories.HuyNQ;
using MangaWorkflow.Services.HuyNQ.DTOs.Chapter;
using MangaWorkflow.Services.HuyNQ.DTOs.Common;
using Mapster;

namespace MangaWorkflow.Services.HuyNQ;

public class ChapterHuyNqService(ChapterHuyNqRepository chapterRepo) : IChapterHuyNqService
{
    private readonly ChapterHuyNqRepository _chapterRepo = chapterRepo;

    public async Task<int> CreateAsync(ChapterCreateRequest chapter)
    {
        try
        {
            var item = chapter.Adapt<ChapterHuyNq>();
            return await _chapterRepo.CreateAsync(item);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var item = await _chapterRepo.GetByIdAsync(id) ?? throw new InvalidDataException("Chapter is not found");
            if (item == null)
                return false;
            return await _chapterRepo.RemoveAsync(item);
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<ChapterHuyNq>> GetAllAsync()
    {
        try
        {
            return await _chapterRepo.GetAllAsync();
        }
        catch
        {
            throw;
        }
    }

    public async Task<ChapterGetByIdResponse?> GetByIdAsync(int id)
    {
        try
        {
            var item = await _chapterRepo.GetByIdAsync(id) ?? throw new InvalidDataException("Chapter is not found");
            var response = item.Adapt<ChapterGetByIdResponse>();
            return response;
        }
        catch
        {
            throw;
        }
    }

    public async Task<PagedResult<ChapterHuyNq>> SearchAsync(ChapterSearchRequest request)
    {
        try
        {
            var (items, totalItems) = await _chapterRepo.SearchAsync(
                request.Title, request.ChapterNumber, request.Approved, request.PageNumber, request.PageSize);

            return new PagedResult<ChapterHuyNq>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems
            };
        }
        catch
        {
            throw;
        }
    }

    public async Task<int> UpdateAsync(int id, ChapterUpdateRequest chapter)
    {
        if (chapter.ChapterMetaHuynqId == null)
        {
            Console.WriteLine("ChapterMetaHuynqId is required for update.");
            return 0;
        }

        var existingChapter = await _chapterRepo.GetByIdAsync(id) ?? throw new InvalidDataException("Chapter is not found");

        chapter.Adapt(existingChapter);
        return await _chapterRepo.UpdateAsync(existingChapter);
    }
}
