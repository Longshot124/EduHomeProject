namespace Edu_Home.DAL.Entities
{
    public class Course: Entity
    {
        public string ImageUrl { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certification { get; set; }
        public DateTime Starts { get;set; }
        public string Duration { get; set; }
        public string ClassDuration { get;set;}
        public string Skill { get; set; }
        public string Language { get; set; }
        public int Students { get; set; }
        public string Assesments { get; set; }
        public int Price { get; set; }
    }
}
