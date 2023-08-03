using Kolmeo.Data;
using Kolmeo.Rest;

namespace Kolmeo.Services
{
    public interface IProductService
    {
        Task<GetProductModel> GetProductModel(int id);
        Task<GetProductsModel> GetProductModels(int count, int skip);
        Task<GetProductModel> SaveProductModel(PostProductModel postProductModel);
        Task<GetProductModel> UpdateProductModel(int id, PostProductModel productModel);
        Task<bool> DeleteProductModel(int id);
    }

    public class ProductService : IProductService
    {
        private IProductRepository _repository;
        private ILogger<ProductService> _logger;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger) 
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> DeleteProductModel(int id)
        {
            return await _repository.DeleteProduct(id);
        }

        public async Task<GetProductModel> GetProductModel(int id)
        {
            var product = await _repository.GetProduct(id);
            if (product != null)
            {
                var result = MapProductToGetProductModel(product);
                return result;
            }
            return null;
        }

        public async Task<GetProductsModel> GetProductModels(int count, int skip)
        {
            var result = new GetProductsModel();
            var products = await _repository.GetProducts(count, skip);
            result.Skip = skip;

            if (products != null)
            {
                result.Count = products.Length;
                foreach (var product in products)
                {
                    result.Products.Add(MapProductToGetProductModel(product));
                }
            }
            result.TotalCount = await _repository.GetProductCount();
            return result;
        }

        public async Task<GetProductModel> SaveProductModel(PostProductModel postProductModel)
        {
            if (postProductModel != null)
            {
                var product = new Product()
                {
                    Name = postProductModel.Name,
                    Description = postProductModel.Description,
                    Price = postProductModel.Price,
                    IsDeleted = false
                };
                var result = await _repository.CreateProduct(product);
                return MapProductToGetProductModel(result);
            }
            return null;
        }

        public async Task<GetProductModel> UpdateProductModel(int id, PostProductModel productModel)
        {
            if (productModel != null)
            {
                var product = new Product()
                {
                    Name = productModel.Name,
                    Description = productModel.Description,
                    Price = productModel.Price,
                    IsDeleted = false
                };
                var result = await _repository.UpdateProduct(product);
                return MapProductToGetProductModel(result);
            }
            return null;
        }

        #region Implementation

        private GetProductModel MapProductToGetProductModel(Product product)
        {
            var result = new GetProductModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsDeleted = product.IsDeleted
            };
            return result;
        }


        #endregion

    }
}
