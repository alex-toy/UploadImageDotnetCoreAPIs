using Microsoft.AspNetCore.Mvc;
using ProductApi.DTO;
using ProductApi.Models;
using ProductApi.Repository.Files;
using ProductApi.Repository.Products;

namespace ProductMiniApi.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepo;

        public ProductController(IFileService fs, IProductRepository productRepo)
        {
            _fileService = fs;
            _productRepo = productRepo;
        }

        [HttpPost]
        public IActionResult Add([FromForm]Product model)
        {
            var status = new StatusDto();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }

            if (model.ImageFile is not null)
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    model.ProductImage = fileResult.Item2; // getting name of image
                }

                bool productResult = _productRepo.Add(model);
                if (productResult)
                {
                    status.StatusCode = 1;
                    status.Message = "Added successfully";
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Error on adding product";
                }
            }

            return Ok(status);
        }
    }
}
