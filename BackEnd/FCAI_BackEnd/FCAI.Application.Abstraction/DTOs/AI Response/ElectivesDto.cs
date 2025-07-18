namespace FCAI.Application.Abstraction.DTOs.AI_Response
{
    public class ElectivesDto
    {
        public int Applied { get; set; }
        public List<string> AppliedOptions { get; set; }
        public int General { get; set; }
        public List<string> GeneralOptions { get; set; }
        public int TotalElectives { get; set; }
        public int UsedElectiveCredits { get; set; }
    }
}
