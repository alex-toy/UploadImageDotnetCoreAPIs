using ProductApi.Models;

namespace ProductApi.Repository.Products
{
    public interface IProductRepository
    {
        bool Add(Product model);
    }
}
