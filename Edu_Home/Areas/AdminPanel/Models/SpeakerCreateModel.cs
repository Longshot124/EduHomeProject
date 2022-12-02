namespace Edu_Home.Areas.AdminPanel.Models
{
    public class SpeakerCreateModel
    {
        public string FullName { get; set; }
        public IFormFile Image { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
    }
}
