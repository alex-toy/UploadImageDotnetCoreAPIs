using Microsoft.AspNetCore.Mvc;
using ProductApi.DTO;
using ProductApi.Models;
using ProductApi.Models.Enums;
using ProductApi.Repository.Files;
using ProductApi.Repository.Products;
using ProductApi.Services.Files;

namespace ProductMiniApi.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public partial class ProductController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepo;

        public ProductController(IFileService fs, IProductRepository productRepo)
        {
            _fileService = fs;
            _productRepo = productRepo;
        }

        [HttpPost]
        public IActionResult Add([FromForm] Product model)
        {
            if (!ModelState.IsValid) return Ok(new StatusDto() { StatusCode = (int)UploadStatusCode.Error, Message = "invalid data" });

            if (model.ImageFile is null) return Ok(new StatusDto() { StatusCode = (int)UploadStatusCode.Error, Message = "no image" });

            StatusDto status = _fileService.SaveImage(model.ImageFile);
            if (status.StatusCode == (int)UploadStatusCode.AddedSuccessfully)
            {
                model.ProductImage = status.Message;
            }

            bool isSuccess = _productRepo.Add(model);

            status = isSuccess
                ? new StatusDto() { StatusCode = (int)UploadStatusCode.AddedSuccessfully, Message = "Added successfully" }
                : new StatusDto() { StatusCode = (int)UploadStatusCode.Error, Message = "Error on adding product" };

            return Ok(status);
        }

        [HttpPost]
        public IActionResult Delete([FromForm] string imageFileName)
        {
            bool isSuccess = _fileService.DeleteImage(imageFileName);

            StatusDto status = isSuccess
                ? new StatusDto() { StatusCode = (int)UploadStatusCode.DeletedSuccessfully, Message = "Deleted successfully" }
                : new StatusDto() { StatusCode = (int)UploadStatusCode.Error, Message = "Error on deleting image product" };

            return Ok(status);
        }
    }
}

