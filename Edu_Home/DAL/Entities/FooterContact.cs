using System.ComponentModel.DataAnnotations;

namespace Edu_Home.DAL.Entities
{
    public class FooterContact : Entity
    {
        public string PhoneNumber { get; set; }
        public string SecondPhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string SecondAddress { get; set; }
        public string Email { get; set; }
    }
}
