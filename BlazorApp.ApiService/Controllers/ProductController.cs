using BlazorApp.Bl.Interfaces.IServices;
using BlazorApp.Models.Entities;
using BlazorApp.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.ApiService.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponseModel>> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(new BaseResponseModel
            {
                Success = true,
                Data = products
            });
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct(ProductModel productModel)
        {
            await _productService.CreateProduct(productModel);
            return Ok(new BaseResponseModel
            {
                Success = true,
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModel>> GetProduct(int id)
        {
            var productModel = await _productService.GetProduct(id);

            if (productModel == null)
            {
                return Ok(new BaseResponseModel
                {
                    Success = false,
                    ErrorMessage = "Not Found"
                });
            }

            return Ok(new BaseResponseModel
            {
                Success = true,
                Data = productModel
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductModel productModel)
        {
            if (id != productModel.ID || !await _productService.ProductModelExists(id))
            {
                return Ok(new BaseResponseModel
                {
                    Success = false,
                    ErrorMessage = "Bad Request"
                });
            }
            await _productService.UpdateProduct(productModel);
            return Ok(new BaseResponseModel
            {
                Success = true
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(!await _productService.ProductModelExists(id))
            {
                return Ok(new BaseResponseModel
                {
                    Success = false,
                    ErrorMessage = "Not Found"
                });
            }
            await _productService.DeleteProduct(id);
            return Ok(new BaseResponseModel
            {
                Success = true
            });
        }
    }
}
