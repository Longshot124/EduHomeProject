using System.ComponentModel.DataAnnotations;

namespace Edu_Home.Areas.AdminPanel.Models
{
    public class TeacherCreateModel
    {
        
        public string FullName { get; set; }
        public string Position { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public byte LanguageSkill { get; set; }
        public byte DesignSkill { get; set; }
        public byte TeamLiderSkill { get; set; }
        public byte InnovationSkill { get; set; }
        public byte DevelopmentSkill { get; set; }
        public byte CommunicationSkill { get; set; }
        public IFormFile Image { get; set; }
    }
}
