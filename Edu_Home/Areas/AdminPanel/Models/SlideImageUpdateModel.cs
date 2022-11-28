namespace Edu_Home.Areas.AdminPanel.Models
{
    public class SlideImageUpdateModel
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public string ?ImageUrl { get; set; } = String.Empty;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ButtonText { get; set; }
    }
}
