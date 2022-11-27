namespace Edu_Home.Areas.AdminPanel.Data
{
    public static class FileExtensions
    {


        public async static Task<string> GenerateFile(this IFormFile file, string rootPath)
        {
            var unicalName = $"{Guid.NewGuid()}-{file.FileName}";
            var path = Path.Combine(rootPath, "img", unicalName);


            var fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);

            return unicalName;
        }
    }
}
