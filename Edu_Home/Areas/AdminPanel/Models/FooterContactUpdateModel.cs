using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Edu_Home.Areas.AdminPanel.Models
{
    public class FooterContactUpdateModel
    {
        public int Id { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? SecondPhoneNumber { get; set; }
        [DataType(DataType.Url)]
        public string Website { get; set; }
        public string Address { get; set; }
        public string? SecondAddress { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
