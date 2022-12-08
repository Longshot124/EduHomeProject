using System.ComponentModel.DataAnnotations;

namespace Edu_Home.Areas.AdminPanel.Models
{
    public class FooterLogoUpdateModel
    {
        public int Id { get; set; }
        public string LogoUrl { get; set; }
        public IFormFile? LogoImage { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Url)]
        public string? FacebookLink { get; set; }
        [DataType(DataType.Url)]
        public string? PisterestLink { get; set; }
        [DataType(DataType.Url)]
        public string? VimeoLink { get; set; }
        [DataType(DataType.Url)]
        public string? TwitterLink { get; set; }
    }
}
