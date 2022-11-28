using System.ComponentModel.DataAnnotations;

namespace Edu_Home.DAL.Entities
{
    public class Slider : Entity
    {
        public string ?Title { get; set; }
        public string ?SubTitle { get; set; }
        public string ?ImageUrl { get; set; }
        public string ?ButtonText { get; set; }


    }
}
