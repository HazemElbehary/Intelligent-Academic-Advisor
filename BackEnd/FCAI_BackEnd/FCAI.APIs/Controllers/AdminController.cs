using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCAI.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController(IAuthService _authService, IServiceManagers _serviceManager) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AdminDto>> Login([FromBody] LoginDto model)
        {
            var result = await _authService.LoginAdminAsync(model);
            return Ok(result);
        }

        [HttpGet("students/{studentId}/recommendation-plan")]
        public async Task<IActionResult> GetStudentRecommendationPlan(int studentId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var recommendationPlan = await _serviceManager.StudentService.GetRecommendedPlanAsync(studentId, adminId, userType: "Admin");
            return Ok(recommendationPlan);
        }

        [HttpGet("courses-with-fails")]
        public async Task<IActionResult> GetCoursesWithFails()
        {
            var coursesWithFails = await _serviceManager.CourseService.GetCoursesWithFailsAsync();
            return Ok(coursesWithFails);
        }
    }
}