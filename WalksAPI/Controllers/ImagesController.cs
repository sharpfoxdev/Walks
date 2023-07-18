using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto) {
            ValidateFileUpload(imageUploadRequestDto);
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var imageDomain = new Image {
                File = imageUploadRequestDto.File,
                FileName = imageUploadRequestDto.FileName,
                FileDescription = imageUploadRequestDto.FileDescription,
                FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                FileSizeInBytes = imageUploadRequestDto.File.Length,
            };
            imageDomain = await imageRepository.UploadAsync(imageDomain);
            return Ok(imageDomain);
        }
        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto) {
            var allowedExtensions = new string[] { ".jpg", "jpeg", ".png" };
            if(!allowedExtensions.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName).ToLower())) {
                ModelState.AddModelError("file", "Invalid file extension");
            }
            long TEN_MB = 10 * 1024 * 1024;
            if(imageUploadRequestDto.File.Length > TEN_MB) {
                ModelState.AddModelError("file", "File size more than 10MB, upload smaller file");
            }
        }
    }
}
