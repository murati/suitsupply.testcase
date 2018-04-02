using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using SuitSupply.TestCase.API.Controllers;
using SuitSupply.TestCase.Business.AppProfiles;
using SuitSupply.TestCase.Business.Helpers;
using SuitSupply.TestCase.Business.Implementations;
using SuitSupply.TestCase.Data.Database;
using SuitSupply.TestCase.Data.DTOs;

namespace SuitSupply.TestCase.Test
{
    [TestFixture]
    public class UnitTest1
    {
        dbTestCase db = new dbTestCase(new DbContextOptionsBuilder<dbTestCase>().UseSqlServer("Data Source=.;User Id=sa;Password=123456;Initial Catalog=sstest").Options);
        ProductsController api;
        ProductHelper business;
        IMapper mapper;
        IMemoryCache cache;
        public UnitTest1()
        {
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AppProfile()));
            mapper = mapConfig.CreateMapper();
        }

        [Test]
        public void EmptyFilterShouldReturnAllItems()
        {
            business = new ProductHelper(db, mapper, cache);
            ICollection<ProductDTO> products = business.Get() as ICollection<ProductDTO>;
            ICollection<ProductDTO> emptyFilteredProducts = business.Filter("") as ICollection<ProductDTO>;

            Assert.AreEqual(products.Count, emptyFilteredProducts.Count);
        }

        [Test]
        [TestCase("trousers"), TestCase("shirts"), TestCase("socks")]
        public void FilterShouldReturnLessThanAllItems(string filter)
        {
            business = new ProductHelper(db, mapper,cache);
            ICollection<ProductDTO> products = business.Get() as ICollection<ProductDTO>;
            ICollection<ProductDTO> emptyFilteredProducts = business.Filter(filter) as ICollection<ProductDTO>;

            //If there are only one unique named product
            Assert.GreaterOrEqual(products.Count, emptyFilteredProducts.Count);
        }

        [Test]
        [TestCase(1)]
        public void DeletingIdShouldBeGeq0(int id)
        {
            business = new ProductHelper(db, mapper,cache);
            bool result = business.Delete(id);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void DeletingIdShouldBeGreaterThan0(int id)
        {
            business = new ProductHelper(db, mapper,cache);
            bool result = business.Delete(id);
            Assert.IsFalse(result);
        }

        [Test]
        public void CrateProduct()
        {

            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            ProductDTO product = new ProductDTO
            {
                Name = "Shirts",
                Price = 10.92M,
                PhotoStream = fileMock.Object,
                Photo = "filename.png"
            };

            api = new ProductsController(db, mapper,cache);
            api.Post(product);
        }

        byte[] GetImageBytes()
        {
            return File.ReadAllBytes(@"C:\Users\murat\Desktop\logo.png");
        }

        [Test]
        public void UpdateProduct()
        {

            ProductDTO product = new ProductDTO
            {
                Id = 1,
                Name = "Trousers",
                Price = 20.92M,
            };

            api = new ProductsController(db, mapper,cache);
            api.Put(product);
        }

    }
}
