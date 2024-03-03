using ProductApi.Models;
using ProductApi.Repo;

namespace ProductApi.Repository.Products
{
    public class ProductService : IProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public bool Add(Product model)
        {
            try
            {
                _context.Product.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
