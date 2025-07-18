using FCAI.Application.Abstraction.DTOs;
using System.Threading.Tasks;

namespace FCAI.Application.Abstraction.IServices
{
    public interface IAuthService
    {
        Task<StudentDto> RegisterAsync(RegisterDto model);
        Task<StudentDto> LoginAsync(LoginDto model);
        Task<AdminDto> LoginAdminAsync(LoginDto model);
    }
} 