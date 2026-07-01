using MangaWorkflow.APIWebApp.HuyNQ.Commons;
using MangaWorkflow.Entities.HuyNQ.Models;
using MangaWorkflow.Services.HuyNQ;
using MangaWorkflow.Services.HuyNQ.DTOs.User;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MangaWorkflow.APIWebApp.HuyNQ.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SystemUserAccountsController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ISystemUserAccountService _userAccountsService;
    private readonly ITokenBlacklistService _tokenBlacklist;

    public SystemUserAccountsController(IConfiguration config, ISystemUserAccountService userAccountsService, ITokenBlacklistService tokenBlacklist)
    {
        _config = config;
        _userAccountsService = userAccountsService;
        _tokenBlacklist = tokenBlacklist;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] GetUserAccountRequest request)
    {
        var response = await _userAccountsService.GetUserAccount(request);

        if (response == null)
            return Unauthorized();

        var user = response.Adapt<SystemUserAccount>();
        var token = GenerateJSONWebToken(user);

        return Ok(token);
    }

    [Authorize]
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        if (string.IsNullOrEmpty(jti))
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string?>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Token does not contain a valid identifier.",
                Data = null
            });
        }

        // Revoke the token until its own expiry so it can no longer be used.
        var expiresUtc = GetTokenExpiryUtc();
        _tokenBlacklist.Revoke(jti, expiresUtc);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string?>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Logged out successfully",
            Data = null
        });
    }

    private DateTime GetTokenExpiryUtc()
    {
        var expClaim = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

        if (long.TryParse(expClaim, out var expSeconds))
            return DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

        // Fallback: mirror the token lifetime used at login.
        return DateTime.UtcNow.AddMinutes(120);
    }

    private string GenerateJSONWebToken(SystemUserAccount systemUserAccount)
    {

        if (systemUserAccount.RoleId == 3)
        {
            return string.Empty;
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        Console.WriteLine(_config["Jwt:Key"]);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                , _config["Jwt:Audience"]
                , new Claim[]
                {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, systemUserAccount.UserName),
                //new(ClaimTypes.Email, systemUserAccount.Email),
                new(ClaimTypes.Role, systemUserAccount.RoleId.ToString()),
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    public sealed record LoginRequest(string UserName, string Password);
}
