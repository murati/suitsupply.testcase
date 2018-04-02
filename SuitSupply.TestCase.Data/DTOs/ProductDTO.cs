using Microsoft.AspNetCore.Http;
using SuitSupply.TestCase.Data.Implementations;
using System;
namespace SuitSupply.TestCase.Data.DTOs
{
    public class ProductDTO : DtoBase
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get { return _lastUpdated.Equals(DateTime.MinValue) ? DateTime.Now : _lastUpdated; }
            set { _lastUpdated = value; }
        }
        public decimal Price { get; set; }
        public IFormFile PhotoStream { get; set; }
    }
}
