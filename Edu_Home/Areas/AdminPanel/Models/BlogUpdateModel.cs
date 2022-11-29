namespace Edu_Home.Areas.AdminPanel.Models
{
    public class BlogUpdateModel
    {   
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } =String.Empty;
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public IFormFile Image { get; set; }
    }
}
