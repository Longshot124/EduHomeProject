namespace Edu_Home.Areas.AdminPanel.Models
{
    public class SpeakerUpdateModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; } = String.Empty;

        public string Profession { get; set; }
        public string Company { get; set; }
    }
}
