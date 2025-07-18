using FCAI.Application.Abstraction;
using FCAI.Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FCAI.APIs.Controllers
{
    [ApiController]
    [Route("api/universities")]
    [EnableCors("AllowDevAndProd")]
    public class UniversityController(IServiceManagers serviceManager): ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<University>> GetAll() 
        {
            var result = await serviceManager.UniversityService.GetAllAsync();
            return Ok(result);
        }
    }
}
