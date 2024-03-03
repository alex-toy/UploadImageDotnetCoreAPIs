using ProductApi.DTO;
using ProductApi.Models.Enums;
using ProductApi.Repository.Files;

namespace ProductApi.Services.Files;

public class FileService : IFileService
{
    private IWebHostEnvironment environment;
    private readonly string[] allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
    public FileService(IWebHostEnvironment env)
    {
        environment = env;
    }

    public StatusDto SaveImage(IFormFile imageFile)
    {
        try
        {
            string contentPath = environment.ContentRootPath;
            string uploadFolderPath = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(uploadFolderPath)) Directory.CreateDirectory(uploadFolderPath);

            string fileExtension = Path.GetExtension(imageFile.FileName);
            if (!allowedExtensions.Contains(fileExtension))
            {
                string errorMessage = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                return new StatusDto() { StatusCode = (int)UploadStatusCode.Error, Message = errorMessage };
            }

            string uniqueString = Guid.NewGuid().ToString();
            string newFileName = uniqueString + fileExtension;
            string fileWithPath = Path.Combine(uploadFolderPath, newFileName);
            FileStream stream = new FileStream(fileWithPath, FileMode.Create);
            imageFile.CopyTo(stream);
            stream.Close();
            return new StatusDto() { StatusCode = (int)UploadStatusCode.AddedSuccessfully, Message = newFileName };
        }
        catch (Exception ex)
        {
            return new StatusDto() { StatusCode = (int)UploadStatusCode.Error, Message = "Error has occured" };
        }
    }

    public bool DeleteImage(string imageFileName)
    {
        try
        {
            string contentPath = environment.ContentRootPath;
            string path = Path.Combine(contentPath, "Uploads\\", imageFileName);

            if (!File.Exists(path))  return false;

            File.Delete(path);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
