using MahdyASP.NETCore.Data;

namespace MahdyASP.NETCore.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<int> CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task<Product> GetProductById(int id);
    }
}
