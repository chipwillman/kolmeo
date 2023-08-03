using Kolmeo.Data;
using Kolmeo.Rest;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Kolmeo.Data
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(int id);
        Task<Product[]> GetProducts(string query, int take, int skip);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<int> GetProductCount();
    }

    public class ProductRepository : IProductRepository
    {
        private KolmeoContext _context;
        private ILogger _logger;
        public ProductRepository(ILogger<ProductRepository> logger)
        {
            _logger = logger;
        }

        public KolmeoContext Context { 
            get {
                if (_context == null)
                {
                    var _contextOptions = new DbContextOptionsBuilder<KolmeoContext>()
                        .UseInMemoryDatabase("Kolmeo")
                        .Options;
                    _context = new KolmeoContext(_contextOptions);
                    _context.Database.EnsureCreated();
                }
                return _context;
            }
            set
            {
                _context = value;
            }
        }
    
        public async Task<bool> DeleteProduct(int id)
        {
            var result = false;
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null )
            {
                if (!product.IsDeleted)
                {
                    product.IsDeleted = true;
                    await Context.SaveChangesAsync();
                    _logger.LogInformation($"Product {id} deleted");
                }
                else
                {
                    _logger.LogInformation($"Product {id} is already deleted");
                }
                result = true;
            }
            else
            {
                _logger.LogError($"Failed to find product to delete id {id}");
            }
            return result;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        public async Task<Product[]> GetProducts(string query, int take, int skip)
        {
            var productsPredicate = Context.Products.Where(p => !p.IsDeleted);
            if (!string.IsNullOrEmpty(query))
            {
                productsPredicate = productsPredicate.Where(p => p.Name.Contains(query) || p.Description.Contains(query));
            }
            var products = await productsPredicate.Skip(skip).Take(take).ToArrayAsync();
            return products;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            Context.Products.Add(product);
            await Context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var originalProduct = await Context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (originalProduct != null)
            {
                originalProduct.Name = product.Name;
                originalProduct.Description = product.Description;
                originalProduct.Price = product.Price;
                await Context.SaveChangesAsync();
                return originalProduct;
            }
            _logger.LogError($"Failed to find product to update id: {product.Id}");
            return null;
        }

        public async Task<int> GetProductCount()
        {
            return await Context.Products.CountAsync();
        }
    }
}
