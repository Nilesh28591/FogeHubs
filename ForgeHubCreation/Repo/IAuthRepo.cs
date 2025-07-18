using ForgeHubCreation.Dto;
using ForgeHubCreation.Model;

namespace ForgeHubCreation.Repo
{
    public interface IAuthRepo
    {
        Task<User> RegistorAsync(RegistorDto registorDto);
        Task<string> LoginAsync(string email, string password);
        Task<bool> VerifyTwoFactorCodeAsync(string email, string code);
    }
}
