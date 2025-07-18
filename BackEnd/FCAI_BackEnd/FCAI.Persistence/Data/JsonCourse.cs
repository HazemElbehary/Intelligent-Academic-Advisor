using FCAI.Domain.Enums;

namespace FCAI.Persistence.Data
{
    class JsonCourse
    {
        public string code { get; set; }
        public string course_name { get; set; }
        public int credit_hours { get; set; }
        public string distribution_category { get; set; }
        public string type { get; set; }
        public string[] prerequisites { get; set; }
        public string Term { get; set; }
        public string department { get; set; }
    }
}
