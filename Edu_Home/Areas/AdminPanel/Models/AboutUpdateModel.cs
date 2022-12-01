namespace Edu_Home.Areas.AdminPanel.Models
{
    public class AboutUpdateModel
    {
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? ButtonText { get; set; }
        public IFormFile Image { get; set; }
    }
}
