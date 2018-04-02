using SuitSupply.TestCase.Data.Database;
using SuitSupply.TestCase.Data.Implementations;
using SuitSupply.TestCase.DataAccess.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SuitSupply.TestCase.DataAccess.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        dbTestCase db;
        public RepositoryBase(dbTestCase _db) => this.db = _db;

        public virtual bool Delete(int id)
        {
            TEntity entity = db.Set<TEntity>().Where(p => p.Id == id).FirstOrDefault();
            var result = false;
            if (entity != null)
            {
                result = db.Set<TEntity>().Remove(entity).State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges(true);
            }
            return result;
        }

        public abstract IEnumerable<TEntity> Filter(string name);

        public virtual IEnumerable<TEntity> GetAll()
        {
            return db.Set<TEntity>().AsEnumerable();
        }

        public virtual TEntity GetById(int id)
        {
            return db.Set<TEntity>().Find(id);
        }

        public virtual int Save(TEntity model)
        {
            db.Set<TEntity>().Add(model);
            return db.SaveChanges(true);
        }

        public virtual int Update(TEntity model)
        {
            TEntity entity = db.Set<TEntity>().Find(model.Id);
            EntityEntry updatingEntry = db.Entry(entity);
            PropertyValues originalValues = db.Entry(entity).OriginalValues;
            PropertyValues currentValues = db.Entry(model).CurrentValues;
            for (int i = 0; i < originalValues.Properties.Count; i++)
            {
                var originalValue = originalValues.Properties[i].PropertyInfo.GetValue(entity);
                var currentValue = currentValues.Properties[i].PropertyInfo.GetValue(model);

                if (currentValue != null && !originalValue.Equals(currentValue))
                {
                    updatingEntry.Property(originalValues.Properties[i].PropertyInfo.Name).CurrentValue = currentValue;
                    updatingEntry.Property(originalValues.Properties[i].PropertyInfo.Name).IsModified = true;
                }
            }
            db.SaveChanges();
            return 0;
        }
    }
}

