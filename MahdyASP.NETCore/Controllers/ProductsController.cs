using System.Security.Claims;
using MahdyASP.NETCore.Authorization;
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
    public class ProductsController(IProductsService productsService) :
        ControllerBase
    {
        private readonly IProductsService _productsService = productsService;

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<Product>> Get()
        {
            var username = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _productsService.GetProducts();
        }

        [HttpGet]
        [Route("{id}")]
        [CheckPermission(Permission.ReadProducts)]
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
