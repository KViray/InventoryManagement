using System.IO;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Images.Services
{
    public interface IImageService
    {
        Task<FileStream> GetImageByFolder(string foldername, string id, string imagename);
    }
}
