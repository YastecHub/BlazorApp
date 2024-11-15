using BlazorApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Bl.Interfaces.IServices
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel> CreateProduct(ProductModel productModel);
        Task<ProductModel> GetProduct(int id);
        Task<bool> ProductModelExists(int id);
        Task UpdateProduct(ProductModel productModel);
    }
}
