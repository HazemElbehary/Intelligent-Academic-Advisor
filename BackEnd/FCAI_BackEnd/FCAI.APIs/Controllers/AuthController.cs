using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCAI.APIs.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableCors("AllowDevAndProd")]
    public class AuthController(IServiceManagers serviceManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            var result = await serviceManager.AuthService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {
            var result = await serviceManager.AuthService.LoginAsync(model);
            return Ok(result);
        }
    }
} 