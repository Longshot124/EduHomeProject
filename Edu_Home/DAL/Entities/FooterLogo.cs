namespace Edu_Home.DAL.Entities
{
    public class FooterLogo : Entity
    {
        public string LogoUrl { get;set; }
        public string Description { get;set; }
        public string? FacebookLink { get;set; }
        public string? PisterestLink { get; set; }   
        public string? VimeoLink { get; set; }
        public string? TwitterLink { get; set; }
    }
}
