using ForgeHubCreation.Dto;
using ForgeHubCreation.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForgeHubCreation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthRepo _authRepo;
        public AuthenticationController(IAuthRepo authRepo)
        {
            _authRepo = authRepo; 
        }
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Register([FromBody] RegistorDto registorDto)
        {
            if (registorDto == null)
            {
                return BadRequest("Invalid registration data.");
            }
            try
            {
                var user = await _authRepo.RegistorAsync(registorDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authRepo.LoginAsync(dto.Email, dto.Password);
            return Ok(new { token });
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA(Verify2FADto dto)
        {
            var isValid = await _authRepo.VerifyTwoFactorCodeAsync(dto.Email, dto.Code);
            return Ok(new { success = isValid });
        }

    }
}
