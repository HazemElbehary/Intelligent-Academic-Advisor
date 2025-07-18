using FCAI.Application.Abstraction;
using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.DTOs.CourseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCAI.APIs.Controllers
{
    [ApiController]
    [Route("api/students")]
    [EnableCors("AllowDevAndProd")]
    public class StudentController(IServiceManagers _serviceManager): ControllerBase
    {
        [Authorize]
        [HttpGet("me/gpa")]
        public async Task<IActionResult> GetStudentGPAAsync()
        {
            var studentId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            var result = await _serviceManager.StudentService.GetStudentGPAAsync(studentId);
            return Ok(result);
        }


        [HttpPost("me/courses")]
        public async Task<ActionResult> AddCoursesToStudent([FromBody] CompletedCoursesDto completedCoursesDto)
        {
            var FCAIIDClaim = User.FindFirstValue(ClaimTypes.PrimarySid);
            await _serviceManager.StudentService.AddCoursesToStudentAsync(completedCoursesDto.CoursesCodes, completedCoursesDto.CourseGrades, FCAIIDClaim);
            return Ok("Courses added to student successfully.");
        }


        [Authorize]
        [HttpGet("me/recommended-plan")]
        public async Task<IActionResult> GetRecommendedPlanAsync()
        {
            var studentId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var userType = User.FindFirstValue("UserType");
            var recommendedPlan = await _serviceManager.StudentService.GetRecommendedPlanAsync(null, studentId, userType);
            return Ok(recommendedPlan);
        }

        [HttpGet("me/available-courses")] 
        public async Task<ActionResult<IEnumerable<ReturnedCourseDto>>> GetCoursesBasedOnStudentId()
        {
            var FCAIIDClaim = User.FindFirstValue(ClaimTypes.PrimarySid);
            var studentUniversityIdClaim = User.FindFirstValue("UniversityId");
            var result = await _serviceManager.StudentService.GetCoursesBasedOnStudentIdAsync(FCAIIDClaim, studentUniversityIdClaim);
            return Ok(result);
        }
    
        [Authorize]
        [HttpPost("department")]
        public async Task<ActionResult> AddDepartmentToStudent(int departmentId)
        {
            var FCAIIDClaim = User.Claims.FirstOrDefault( c => c.Type == ClaimTypes.PrimarySid)?.Value;
            await _serviceManager.StudentService.AddDepartmentToStudentAsync(departmentId, FCAIIDClaim);
            return Ok("Department added to student successfully.");
        }

        [Authorize]
        [HttpPatch("term")]
        public async Task<ActionResult> UpdateTermOfStudent(int termId)
        {
            var FCAIIDClaim = User.Claims.FirstOrDefault( c => c.Type == ClaimTypes.PrimarySid)?.Value;
            await _serviceManager.StudentService.UpdateTermOfStudentAsync(termId, FCAIIDClaim);
            return Ok("Term's updated successfully.");
        }
    }
}
