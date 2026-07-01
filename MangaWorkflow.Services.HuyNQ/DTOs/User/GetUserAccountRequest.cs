using System.ComponentModel.DataAnnotations;

namespace MangaWorkflow.Services.HuyNQ.DTOs.User;

public class GetUserAccountRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Email must be between 1 and 50 characters.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Password must be between 1 and 100 characters.")]
    public string Password { get; set; } = default!;
}
