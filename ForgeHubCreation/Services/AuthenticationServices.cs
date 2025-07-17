using ForgeHubCreation.Data;
using ForgeHubCreation.Dto;
using ForgeHubCreation.Model;
using ForgeHubCreation.Repo;

namespace ForgeHubCreation.Services
{
    public class AuthenticationServices : IAuthRepo
    {
        private readonly ApplicationDbContext _context;
        public AuthenticationServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User> RegistorAsync(RegistorDto registorDto)
        {
            throw new NotImplementedException();
        }
    }
}
