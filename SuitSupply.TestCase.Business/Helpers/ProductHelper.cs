using SuitSupply.TestCase.Business.Implementations;
using SuitSupply.TestCase.Data.Database;
using SuitSupply.TestCase.Data.DTOs;
using SuitSupply.TestCase.Data.Models;
using SuitSupply.TestCase.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using AutoMapper;
using OfficeOpenXml;
using Microsoft.Extensions.Caching.Memory;

namespace SuitSupply.TestCase.Business.Helpers
{
    public class ProductHelper : IProductHelper
    {
        readonly string ImagesDir = "productImages";
        ProductRepository repository;
        IMapper mapper;
        IMemoryCache cache;

        public ProductHelper(dbTestCase db, IMapper mapper, IMemoryCache cache)
        {
            repository = new ProductRepository(db);
            this.mapper = mapper;
            this.cache = cache;
        }

        public IEnumerable<ProductDTO> Get()
        {
            IEnumerable<ProductDTO> results;
            if (!cache.TryGetValue(CacheKeys.AllProducts, out results))
            {
                IEnumerable<Product> products = repository.GetAll();
                results = mapper.Map<IEnumerable<ProductDTO>>(products);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
                cache.Set(CacheKeys.AllProducts, results, cacheEntryOptions);
            }
            results = cache.Get<IEnumerable<ProductDTO>>(CacheKeys.AllProducts);
            return results;
        }

        public ProductDTO GetById(int id)
        {
            if (id <= 0)
                return null;

            Product product = repository.GetById(id);
            return mapper.Map<ProductDTO>(product);
        }

        public bool Update(ProductDTO model)
        {
            var product = mapper.Map<ProductDTO, Product>(model);

            var result = repository.Update(product);
            return result > 0;
        }

        public IEnumerable<ProductDTO> Filter(string name)
        {
            IEnumerable<Product> products = repository.Filter(name);
            IEnumerable<ProductDTO> results = mapper.Map<IEnumerable<ProductDTO>>(products);
            return results;
        }

        public bool SaveForm(ProductDTO model)
        {
            var result = 0;
            Product product = mapper.Map<Product>(model);
            string imageName = UploadImage(model);
            if (!string.IsNullOrEmpty(imageName))
            {
                product.Photo = imageName;
            }
            result = repository.Save(product);
            return result > 0;
        }

        public string UploadImage(ProductDTO model)
        {
            if (model.PhotoStream == null)
                return null;
            string extension = Path.GetExtension(model.PhotoStream.FileName);
            string imageName = $"{Guid.NewGuid().ToString()}{extension}";
            string result = string.Empty;
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                byte[] contents;
                using (MemoryStream ms = new MemoryStream())
                {
                    model.PhotoStream.OpenReadStream().CopyTo(ms);
                    contents = ms.ToArray();
                }
                File.WriteAllBytes($"{path}\\{imageName}", contents);
                result = imageName;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public bool Delete(int id)
        {
            int itemId = 0;
            if (int.TryParse(id.ToString(), out itemId))
            {
                if (itemId >= 1)
                    return repository.Delete(itemId);
                else return false;
            }
            else
                return false;
        }

        public byte[] ExportToExcel(string filter)
        {
            IEnumerable<Product> products = repository.Filter(filter);
            byte[] contents;
            using (MemoryStream stream = new MemoryStream())
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Products Filter - {filter}");
                    worksheet.Cells.LoadFromCollection<Product>(products, true);
                    package.Save(); //Save the workbook.
                }
                contents = stream.ToArray();
            }
            return contents;
        }
    }
}
