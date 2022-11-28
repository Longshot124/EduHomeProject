namespace Edu_Home.DAL.Entities
{
    public class Blog : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
    }
}
