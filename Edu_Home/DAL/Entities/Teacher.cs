namespace Edu_Home.DAL.Entities
{
    public class Teacher : Entity
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; } 
        public string Position { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }

        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public string LanguageSkill { get; set; }
        public string DesignSkill { get; set; }
        public string TeamLiderSkill { get; set; }
        public string InnovationSkill { get;set; }
        public string DevelopmentSkill { get; set; }
        public string CommunicationSkill { get; set; }


    }
}
