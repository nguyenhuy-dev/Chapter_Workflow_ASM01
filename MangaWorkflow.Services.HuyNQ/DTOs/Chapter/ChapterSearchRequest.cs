using System.ComponentModel.DataAnnotations;

namespace MangaWorkflow.Services.HuyNQ.DTOs.Chapter;

public record ChapterSearchRequest(
    [property: StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
    string? Title,

    [property: Range(1, int.MaxValue, ErrorMessage = "ChapterNumber must be a positive number.")]
    int? ChapterNumber,

    bool? Approved)
{
}
