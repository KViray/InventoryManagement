using CloudinaryDotNet;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Images.Services
{
    internal class ImageService : IImageService
    {
        public async Task<FileStream> GetImageByFolder(string foldername, string id, string imagename)
        {
            var image = Path.Combine(Directory.GetCurrentDirectory(), "Resources", foldername, id, imagename);

            try
            {
                return await Task.FromResult(File.Open(image, FileMode.Open));
            }
            catch
            {
                return null;
            }
        }
    }
}
