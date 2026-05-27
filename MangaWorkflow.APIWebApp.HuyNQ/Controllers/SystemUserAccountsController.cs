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

    public SystemUserAccountsController(IConfiguration config, ISystemUserAccountService userAccountsService)
    {
        _config = config;
        _userAccountsService = userAccountsService;
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
