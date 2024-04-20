using MahdyASP.NETCore.Data;
using Microsoft.EntityFrameworkCore;

namespace MahdyASP.NETCore.Services
{
    public class ProductsService(ApplicationDBContext applicationDBContext) :
        IProductsService
    {
        private readonly ApplicationDBContext _applicationDBContext = 
            applicationDBContext;

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _applicationDBContext.Set<Product>().ToListAsync();
        }

        public async Task<int> CreateProduct(Product product)
        {            
            _applicationDBContext.Set<Product>().Add(product);
            await _applicationDBContext.SaveChangesAsync();

            return product.Id;
        }

        public async Task UpdateProduct(Product product)
        {
            var existingProduct = await _applicationDBContext.Set<Product>().FindAsync(product.Id);

            existingProduct.Name =  product.Name;
            existingProduct.Sku = product.Sku;

            await _applicationDBContext.SaveChangesAsync();            
        }

        public async Task DeleteProduct(int id)
        {
            var existingProduct = await _applicationDBContext.Set<Product>().FindAsync(id);

            _applicationDBContext.Set<Product>().Remove(existingProduct);

            await _applicationDBContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _applicationDBContext.Set<Product>().FindAsync(id);
        }
    }
}
