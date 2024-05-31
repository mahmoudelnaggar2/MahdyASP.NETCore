using System.Security.Claims;
using MahdyASP.NETCore.Data;
using MahdyASP.NETCore.Filters;
using MahdyASP.NETCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MahdyASP.NETCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [LogSensitiveAction]
    [SensitiveActionsLogger]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet(Name = "GetProducts")]
        //[LogSensitiveAction]
        public async Task<IEnumerable<Product>> Get()
        {
            var username = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
