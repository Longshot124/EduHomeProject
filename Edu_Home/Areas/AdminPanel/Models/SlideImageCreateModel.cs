using System.ComponentModel.DataAnnotations;

namespace Edu_Home.Areas.AdminPanel.Models
{
    public class SlideImageCreateModel
    {
        public IFormFile Image { get; set; }
        
        public string ?Title { get; set; }

        public string ?SubTitle { get; set; }
        
        public string ?ButtonText { get; set; }

        
    }
}
