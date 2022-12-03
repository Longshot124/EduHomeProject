using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Edu_Home.Areas.AdminPanel.Models
{
    public class EventUpdateModel
    {
        public IFormFile Image { get; set; }
        public string ImageUrl { get;set; } = String.Empty;
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }
        public string Adress { get; set; }
        public string Content { get; set; }
        public List<SelectListItem>? Speakers { get; set; }
        public List<int> SpeakersId { get; set; }
    }
}
