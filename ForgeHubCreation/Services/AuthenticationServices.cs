using ForgeHubCreation.Data;
using ForgeHubCreation.Dto;
using ForgeHubCreation.Model;
using ForgeHubCreation.Repo;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForgeHubCreation.Services
{
    public class AuthenticationServices : IAuthRepo
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public AuthenticationServices(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User> RegistorAsync(RegistorDto registordto)
        {
           var existinguser=_context.Users.FirstOrDefault(u=>u.Email==registordto.Email);
           if(existinguser != null)
            {
                throw new Exception("User already exists with this email");
            }
            var secreteKey = KeyGeneration.GenerateRandomKey(20);
            var base32secret = Base32Encoding.ToString(secreteKey);
            var newuser = new User
            {
                FirstName = registordto.FirstName,
                LastName = registordto.LastName,
                Email = registordto.Email,
                PasswordHash=BCrypt.Net.BCrypt.HashPassword(registordto.PasswordHash),
                PhoneNumber = registordto.PhoneNumber,
                Role = registordto.Role,
                TwoFactorKey = base32secret
            };
            _context.Users.Add(newuser);
            await _context.SaveChangesAsync();
            return newuser;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid email or password");

            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<bool> VerifyTwoFactorCodeAsync(string email, string code)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            var totp = new Totp(Base32Encoding.ToBytes(user.TwoFactorKey));
            return await Task.FromResult(totp.VerifyTotp(code, out _, new VerificationWindow(2, 2)));
        }
    }
}
