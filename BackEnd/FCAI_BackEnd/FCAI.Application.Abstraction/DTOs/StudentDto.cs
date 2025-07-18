namespace FCAI.Application.Abstraction.DTOs
{
    public class StudentDto
    {
        public int FCAIID { get; set; }
        public required string UserName { get; set; }
        public required string University { get; set; }
        public required string Token { get; set; }
    }
}
