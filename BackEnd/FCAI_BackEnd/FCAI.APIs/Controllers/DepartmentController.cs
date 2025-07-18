using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.DTOs;
using FCAI.Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FCAI.APIs.Controllers
{
    [ApiController]
    [Route("api/departments")]
    [EnableCors("AllowDevAndProd")]
    public class DepartmentController(IServiceManagers serviceManager): ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ReturnedDepartmentDto>> GetAll()
        {
            var result = await serviceManager.DepartmentService.GetAllAsync();
            return Ok(result);
        }
    }
}
