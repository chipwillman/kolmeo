using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Kolmeo.Data;
using Kolmeo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kolmeo.Tests.Business
{
    [TestClass]
    public class ProductServicesTests
    {
        

        [TestMethod]
        public async Task TestGetProduct()
        {
            var loggingMock = new Mock<ILogger<ProductService>>();
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(r => r.GetProduct(It.IsAny<int>())).Returns(Task.FromResult(new Product {  Id = 1, Name = "pen", Description = "a ball point pen", Price = 2.49m, IsDeleted = false }));
            var service = new ProductService(repositoryMock.Object, loggingMock.Object);

            var product = await service.GetProductModel(1);
            Assert.IsNotNull(product);
            Assert.AreEqual("pen", product.Name);
            Assert.AreEqual("a ball point pen", product.Description);
            Assert.AreEqual(true, product.IsDeleted.HasValue);
            Assert.AreEqual(false, product.IsDeleted.Value);
        }

        public async Task TestDeleteProduct()
        {
            var loggingMock = new Mock<ILogger<ProductService>>();
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(r => r.GetProduct(It.IsAny<int>())).Returns(Task.FromResult(new Product { Id = 1, Name = "pen", Description = "a ball point pen", Price = 2.49m, IsDeleted = false }));
            repositoryMock.Setup(r => r.DeleteProduct(It.IsAny<int>())).Returns(Task.FromResult(true));
            var service = new ProductService(repositoryMock.Object, loggingMock.Object);

            var result = await service.DeleteProductModel(1);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TestGetProducts()
        {
            var loggingMock = new Mock<ILogger<ProductService>>();
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(r => r.GetProducts(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new[] { new Product { Id = 1, Name = "blue pen", Description = "a blue ball point pen", Price = 2.49m, IsDeleted = false },
                new Product { Id = 2, Name = "green pen", Description = "a green ball point pen", Price = 2.89m, IsDeleted = false },
                new Product { Id = 3, Name = "red pen", Description = "a red ball point pen", Price = 2.59m, IsDeleted = false },
                new Product { Id = 4, Name = "black pen", Description = "a black ball point pen", Price = 2.69m, IsDeleted = false }  }));
            repositoryMock.Setup(r => r.GetProductCount()).Returns(Task.FromResult(24));
            var service = new ProductService(repositoryMock.Object, loggingMock.Object);

            var product = await service.GetProductModels("pen", 4, 20);
            Assert.AreEqual(4, product.Count);
            Assert.AreEqual(24, product.TotalCount); 
        }
    }
}
