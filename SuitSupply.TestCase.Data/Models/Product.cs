using System;
using SuitSupply.TestCase.Data.Implementations;

namespace SuitSupply.TestCase.Data.Models
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
