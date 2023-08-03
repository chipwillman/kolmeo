using Kolmeo.Data;
using Kolmeo.Rest;
using Kolmeo.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kolmeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        private ILogger _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Get a paged list of all products
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GetProductsModel>> GetProducts(int count = 20, int offset = 0)
        {
            _logger.LogInformation($"GetProducts called with count {count} and offset {offset}");
            var products = await _productService.GetProductModels(count, offset);
            return Ok(products);
        }

        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductModel>> GetProduct(int id)
        {
            _logger.LogInformation($"GetProduct called with id {id}");
            var product = await _productService.GetProductModel(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        /// <summary>
        /// Create a Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The newly created Product</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Products
        ///     {
        ///         "name": "product name"
        ///         "description: "A description of the produce"
        ///         "price": 45.4545
        ///     }
        /// </remarks>
        /// <response code="200">Returns the newly created product</response>
        /// <response code="400">The product is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostProduct([FromBody] PostProductModel product)
        {
            _logger.LogInformation($"PostProductModel called with name {product.Name} description: {product.Description}");
            var result = await _productService.SaveProductModel(product);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Soft deletes a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">The product is successfully deleted</response>
        /// <response code="400">The product was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"DeleteProduct called for id {id}");
            var result = await _productService.DeleteProductModel(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productModel"></param>
        /// <returns></returns>
        /// <response code="200">The product was successfully updated</response>
        /// <response code="400">The product was not found</response>
        [HttpPut]
        public async Task<ActionResult<GetProductModel>> UpdateProduct(int id, PostProductModel productModel)
        {
            _logger.LogInformation($"Updating Product ${id}");
            var result = await _productService.UpdateProductModel(id, productModel);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();

        }
    }
}
