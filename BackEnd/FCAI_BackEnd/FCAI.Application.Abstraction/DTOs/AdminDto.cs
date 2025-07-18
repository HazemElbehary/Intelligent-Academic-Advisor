namespace FCAI.Application.Abstraction.DTOs
{
    public class AdminDto
    {
        public int AdminID { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string Position { get; set; }
        public string? Department { get; set; }
        public required string Token { get; set; }
    }
} 