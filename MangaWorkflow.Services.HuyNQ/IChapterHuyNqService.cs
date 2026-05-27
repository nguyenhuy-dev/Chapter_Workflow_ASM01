using MangaWorkflow.Entities.HuyNQ.Models;

namespace MangaWorkflow.Services.HuyNQ;

public interface IChapterHuyNqService
{
    Task<List<ChapterHuyNq>> GetAllAsync();
    Task<ChapterHuyNq?> GetByIdAsync(int id);
    Task<List<ChapterHuyNq>> SearchAsync(string? title, int? chapterNumber, bool? approved);

    Task<int> CreateAsync(ChapterHuyNq chapter);
    Task<int> UpdateAsync(ChapterHuyNq chapter);
    Task<bool> DeleteAsync(int id);
}
