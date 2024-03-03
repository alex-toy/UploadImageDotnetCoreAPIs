using ProductApi.DTO;

namespace ProductApi.Repository.Files
{
    public interface IFileService
    {
        public StatusDto SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
