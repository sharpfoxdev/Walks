using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories {
    public class LocalImageRepository : IImageRepository {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly WalksDbContext walksDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            WalksDbContext walksDbContext)
        {
            // to get access to images folder on file system
            this.webHostEnvironment = webHostEnvironment;
            // to create URL to image we uploaded 
            // https://localhost:1234/images/image.jpg
            this.httpContextAccessor = httpContextAccessor;
            // db context to save changes to SQL database
            this.walksDbContext = walksDbContext;
        }
        public async Task<Image> UploadAsync(Image image) {
            var localFilePath = Path.Combine(
                webHostEnvironment.ContentRootPath, 
                "Images", 
                $"{image.FileName}{image.FileExtension}"
                );
            // uploading image
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // create url file path
            // https://localhost:1234/images/image.jpg
            string scheme = httpContextAccessor.HttpContext.Request.Scheme;
            string host = httpContextAccessor.HttpContext.Request.Host.ToString();
            string pathBase = httpContextAccessor.HttpContext.Request.PathBase;
            var urlFilePath = 
                $"{scheme}://{host}{pathBase}/Images/{image.FileName}{image.FileExtension}";
            
            image.FilePath = urlFilePath;

            // Save to the database (Image table)
            await walksDbContext.Images.AddAsync(image);
            await walksDbContext.SaveChangesAsync();

            return image;
        }
    }
}
