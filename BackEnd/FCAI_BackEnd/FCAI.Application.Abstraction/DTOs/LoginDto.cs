namespace FCAI.Application.Abstraction.DTOs
{
    public class LoginDto
    {
        public int FCAIID { get; set; }
        public required string Password { get; set; }
    }
}
