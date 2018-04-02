using SuitSupply.TestCase.Data.Database;
using SuitSupply.TestCase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SuitSupply.TestCase.DataAccess.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
    {
        dbTestCase db;
        public ProductRepository(dbTestCase db) : base(db)
        {

        }

        public override IEnumerable<Product> Filter(string name)
        {
            return base.GetAll().Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
