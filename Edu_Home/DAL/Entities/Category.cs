namespace Edu_Home.DAL.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();

    }
}
