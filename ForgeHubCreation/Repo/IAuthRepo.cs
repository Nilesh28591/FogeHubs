using ForgeHubCreation.Dto;
using ForgeHubCreation.Model;

namespace ForgeHubCreation.Repo
{
    public interface IAuthRepo
    {
        Task<User> RegistorAsync(RegistorDto registorDto);
    }
}
