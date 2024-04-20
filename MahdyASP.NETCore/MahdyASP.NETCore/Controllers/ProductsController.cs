using MahdyASP.NETCore.Data;
using MahdyASP.NETCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace MahdyASP.NETCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _productsService.GetProducts();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Product> GetById(int id)
        {
            return await _productsService.GetProductById(id);
        }

        [HttpPost]
        public async Task<int> CreateProduct(Product product)
        {
            return await _productsService.CreateProduct(product);
        }

        [HttpPut]
        public async Task UpdateProduct(Product product)
        {
            await _productsService.UpdateProduct(product);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteProduct(int id)
        {
            await _productsService.DeleteProduct(id);
        }
    }
}
