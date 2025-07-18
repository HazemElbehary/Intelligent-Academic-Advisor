using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.DTOs.CourseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCAI.APIs.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/courses")]
    public class CourseController(IServiceManagers serviceManager) : ControllerBase
    {
    }
}
