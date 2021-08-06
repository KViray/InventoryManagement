using InventoryManagement.Features.Images.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Images
{
    [ApiController]
    [Route("api/")]
    public class ImageController: ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [HttpGet("{foldername}/{id}/{imagename}")]
        public async Task<dynamic> GetImageByFolder(string foldername, string id, string imagename)
        {
            var image = await _imageService.GetImageByFolder(foldername, id, imagename);
            if (image == null)
            {

                return Ok("Folder not found");
                
            }
            return File(image, "image/jpeg");
        }
    }
}
