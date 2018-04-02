using System;
using System.Collections.Generic;
using System.Text;
namespace SuitSupply.TestCase.DataAccess.Implementations
{
    interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Filter(string name);
        TEntity GetById(int id);
        int Save(TEntity model);
        int Update(TEntity model);
        bool Delete(int id);
    }
}
