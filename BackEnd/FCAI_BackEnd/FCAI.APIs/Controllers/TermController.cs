using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FCAI.APIs.Controllers
{
    [ApiController]
    [Route("api/terms")]
    [EnableCors("AllowDevAndProd")]
    [Authorize]
    public class TermController(IServiceManagers _serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ReturnedTermDto>> GetAll()
        {
            var result = await _serviceManager.TermService.GetAllAsync();
            return Ok(result);
        }
    }
}