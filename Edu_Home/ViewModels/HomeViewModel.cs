using Edu_Home.DAL.Entities;

namespace Edu_Home.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        public List<About> Abouts { get; set; } = new List<About>();
        public List<Course> Courses { get; set; } = new List<Course> ();
        public List<Speaker> Speaker { get; set; }= new List<Speaker> ();
        public List<Event> Events { get; set; } = new List<Event> ();
        
    }
}
