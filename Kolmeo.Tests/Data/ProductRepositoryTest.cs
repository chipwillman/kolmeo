using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Kolmeo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kolmeo.Tests.Data
{
    [TestClass]
    public class ProductRepositoryTest
    {
        [TestInitialize]
        public void Init()
        {
            var _contextOptions = new DbContextOptionsBuilder<KolmeoContext>()
            .UseInMemoryDatabase("Kolmeo")
            .Options;

            _context = new KolmeoContext(_contextOptions);

        }

        KolmeoContext _context;

        [TestMethod]
        public async Task TestCreateProduct()
        {
            var mockLogger = new Mock<ILogger<ProductRepository>>();
            var repo = new ProductRepository(mockLogger.Object) { Context = _context };
            var product = new Product { Name = "blue pen", Description = "a blue point pen", Price = 2.49m };
            await repo.CreateProduct(product);
            Assert.AreEqual(1, await repo.GetProductCount());
        }

        [TestMethod]
        public async Task TestUpdateProduct()
        {
            var mockLogger = new Mock<ILogger<ProductRepository>>();
            var repo = new ProductRepository(mockLogger.Object) { Context = _context };
            var product = new Product { Name = "blue pen", Description = "a blue point pen", Price = 2.49m };
            var savedProduct = await repo.CreateProduct(product);
            var updatedProduct = new Product {Id = product.Id, Name = "red pen", Description = "a red point pen", Price = 2.59m };
            await repo.UpdateProduct(updatedProduct);
            Assert.AreNotEqual("red pen", savedProduct.Description);

        }
    }
}
