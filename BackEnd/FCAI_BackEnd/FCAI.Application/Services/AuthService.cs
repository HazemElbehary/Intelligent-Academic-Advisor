using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.Exceptions;
using FCAI.Application.Abstraction.IServices;
using FCAI.Domain.Entities;
using FCAI.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FCAI.Application.Services
{
    public class AuthService(
        UserManager<Student> userManager,
        SignInManager<Student> signInManager,
        IOptions<JwtSettings> jwtSettings) : IAuthService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<StudentDto> RegisterAsync(RegisterDto model)
        {
            var conflict = await userManager.Users
                .FirstOrDefaultAsync(u => u.FCAIID == model.FCAIID || u.UserName == model.UserName);

            if (conflict != null)
            {
                var field = conflict.FCAIID == model.FCAIID ? "FCAI ID" : "UserName";
                throw new ApiExceptionResponse(400, $"{field} is already taken.");
            }

            var user = new Student
            {
                FCAIID = model.FCAIID,
                UserName = model.UserName,
                UserType = UserType.Student,
                UniversityId = model.UniversityId,
                DepartmentId = model.DepartmentId,
                StudentTerm = (StudentTerm)model.UserTerm,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ApiExceptionResponse(400, message, string.Empty);
            }

            // Add student role
            await userManager.AddToRoleAsync(user, "Student");

            var created = await userManager.Users
                .Include(u => u.StudentUniversity)
                .FirstOrDefaultAsync(u => u.FCAIID == user.FCAIID);

            return new StudentDto
            {
                FCAIID = created.FCAIID,
                UserName = created.UserName,
                University = created.StudentUniversity?.Name ?? "N/A",
                Token = await GenerateTokenAsync(user, "Student")
            };
        }

        public async Task<StudentDto> LoginAsync(LoginDto model)
        {
            var student = await userManager.Users
                .Include(u => u.StudentUniversity)
                .FirstOrDefaultAsync(s => s.FCAIID == model.FCAIID && s.UserType == UserType.Student);

            if (student is null)
                throw new ApiExceptionResponse(404, "Student not found. Please Register");

            var result = await signInManager.CheckPasswordSignInAsync(student, model.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
                throw new ApiExceptionResponse(403, "Account Is Locked.");

            if (!result.Succeeded)
                throw new ApiExceptionResponse(403, "Invalid Login.");

            return new StudentDto
            {
                FCAIID = student.FCAIID,
                UserName = student.UserName,
                University = student.StudentUniversity?.Name ?? "N/A",
                Token = await GenerateTokenAsync(student, "Student")
            };
        }

        public async Task<AdminDto> LoginAdminAsync(LoginDto model)
        {
            var admin = await userManager.Users
                .FirstOrDefaultAsync(s => s.FCAIID == model.FCAIID && s.UserType == UserType.Admin);

            if (admin is null)
                throw new ApiExceptionResponse(404, "Admin not found.");

            var result = await signInManager.CheckPasswordSignInAsync(admin, model.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
                throw new ApiExceptionResponse(403, "Account Is Locked.");

            if (!result.Succeeded)
                throw new ApiExceptionResponse(403, "Invalid Login.");

            return new AdminDto
            {
                AdminID = admin.FCAIID,
                UserName = admin.UserName,
                FullName = admin.FullName,
                Position = admin.Position,
                Department = admin.Department?.Name,
                Token = await GenerateTokenAsync(admin, "Admin")
            };
        }

        private async Task<string> GenerateTokenAsync(Student user, string role)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim(ClaimTypes.PrimarySid, user.FCAIID.ToString()));
            userClaims.Add(new Claim("UserType", user.UserType.ToString()));
            userClaims.Add(new Claim(ClaimTypes.Role, role));

            if (user.UniversityId.HasValue)
                userClaims.Add(new Claim("UniversityId", user.UniversityId.ToString()));

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer: _jwtSettings.Issure,
                audience: _jwtSettings.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }
    }
}