namespace Edu_Home.Areas.AdminPanel.Models
{
    public class BlogCreateModel
    {
        public string ?Title { get; set; }
        public string ?Description { get; set; }
        
        public string ?Author { get; set; }
        public DateTime Created { get; set; }
        public IFormFile Image { get; set; }

        
    }
}
