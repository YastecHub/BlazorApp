using BlazorApp.Bl.Interfaces.IRepository;
using BlazorApp.Database.Data;
using BlazorApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Bl.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductModel> CreateProduct(ProductModel productModel)
        {
           _dbContext.Products.AddAsync(productModel);
            await _dbContext.SaveChangesAsync();

            return productModel;
        }

        public Task<List<ProductModel>> Getproducts()
        {
             return _dbContext.Products.ToListAsync();
        }

        public Task<ProductModel> GetProduct(int id)
        {
           return _dbContext.Products.FirstOrDefaultAsync(n => n.ID == id);
        }

        public Task<bool> ProductModelExists(int id)
        {
           return _dbContext.Products.AnyAsync(e => e.ID == id);
        }

        public async Task UpdateProduct(ProductModel productModel)
        {
          _dbContext.Entry(productModel).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(n => n.ID == id);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
