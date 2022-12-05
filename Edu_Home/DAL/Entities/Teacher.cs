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
        public byte LanguageSkill { get; set; }
        public byte DesignSkill { get; set; }
        public byte TeamLiderSkill { get; set; }
        public byte InnovationSkill { get;set; }
        public byte DevelopmentSkill { get; set; }
        public byte CommunicationSkill { get; set; }


    }
}
