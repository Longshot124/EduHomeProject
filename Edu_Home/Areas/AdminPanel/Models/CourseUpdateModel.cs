using Edu_Home.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Edu_Home.Areas.AdminPanel.Models
{
    public class CourseUpdateModel
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();     
        public string ImageUrl { get; set; } = String.Empty;
        public int CategoryId { get; set; } = new();
        public string Title { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certification { get; set; }
        public DateTime Starts { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string Skill { get; set; }
        public string Language { get; set; }
        public int Students { get; set; }
        public string Assesments { get; set; }
        public int Price { get; set; }
    }
}
