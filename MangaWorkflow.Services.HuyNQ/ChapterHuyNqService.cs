using MangaWorkflow.Entities.HuyNQ.Models;
using MangaWorkflow.Repositories.HuyNQ;

namespace MangaWorkflow.Services.HuyNQ;

public class ChapterHuyNqService(ChapterHuyNqRepository chapterRepo) : IChapterHuyNqService
{
    private readonly ChapterHuyNqRepository _chapterRepo = chapterRepo;

    public async Task<int> CreateAsync(ChapterHuyNq chapter)
    {
        try
        {
            return await _chapterRepo.CreateAsync(chapter);
        }
        catch (Exception)
        {

        }

        return 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var item = await _chapterRepo.GetByIdAsync(id);
            if (item == null)
                return false;
            return await _chapterRepo.RemoveAsync(item);
        }
        catch (Exception)
        {

        }

        return false;
    }

    public async Task<List<ChapterHuyNq>> GetAllAsync()
    {
        try
        {
            return await _chapterRepo.GetAllAsync();
        }
        catch
        {

        }

        return [];
    }

    public async Task<ChapterHuyNq?> GetByIdAsync(int id)
    {
        try
        {
            return await _chapterRepo.GetByIdAsync(id);
        }
        catch
        {

        }

        return null;
    }

    public async Task<List<ChapterHuyNq>> SearchAsync(string? title, int? chapterNumber, bool? approved)
    {
        try
        {
            return await _chapterRepo.SearchAsync(title, chapterNumber, approved);
        }
        catch
        {

        }

        return [];
    }

    public Task<int> UpdateAsync(ChapterHuyNq chapter)
    {
        throw new NotImplementedException();
    }
}
