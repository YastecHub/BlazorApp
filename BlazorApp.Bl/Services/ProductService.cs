using BlazorApp.Bl.Interfaces.IRepository;
using BlazorApp.Bl.Interfaces.IServices;
using BlazorApp.Models.Entities;

namespace BlazorApp.Bl.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<ProductModel> CreateProduct(ProductModel productModel)
        {
            return _productRepository.CreateProduct(productModel);
        }

        public Task<List<ProductModel>> GetProducts()
        {
           return _productRepository.Getproducts();
        }

        public Task<ProductModel> GetProduct(int id)
        {
           return _productRepository.GetProduct(id);
        }

        public Task<bool> ProductModelExists(int id)
        {
            return _productRepository.ProductModelExists(id);
        }

        public Task UpdateProduct(ProductModel productModel)
        {
           return _productRepository.UpdateProduct(productModel);
        }
    }
}
