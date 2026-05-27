using MangaWorkflow.Entities.HuyNQ.Models;
using MangaWorkflow.Repositories.HuyNQ;
using MangaWorkflow.Services.HuyNQ.DTOs.User;
using Mapster;

namespace MangaWorkflow.Services.HuyNQ;

public class SystemUserAccountService(SystemUserAccountRepository userRepo)
{
    private readonly SystemUserAccountRepository _userRepo = userRepo;

    public async Task<GetUserAccountResponse?> GetUserAccount(GetUserAccountRequest request)
    {
        try
        {
            var user = await Task.Run(() => _userRepo.GetUserAccount(request.UserName, request.Password));
            return user.Adapt<GetUserAccountResponse>();
        }
        catch (Exception) { }

        return null;
    }
}
