using System.ComponentModel.DataAnnotations;

namespace MangaWorkflow.Services.HuyNQ.DTOs.Chapter;

public record ChapterSearchRequest(
    [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
    string? Title,

    [Range(1, int.MaxValue, ErrorMessage = "ChapterNumber must be a positive number.")]
    int? ChapterNumber,

    bool? Approved,

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
    int PageNumber = 1,

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    int PageSize = 10)
{
}
